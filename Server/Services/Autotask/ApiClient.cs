using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Nexus.Shared.Models.Autotask;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serializers.SystemTextJson;

namespace Nexus.Server.Services.Autotask
{
    public class ApiClient
    {
        public const int MAX_PAGE_SIZE = 500;
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;
        private readonly CancellationToken _cancellationToken = new CancellationToken(false);
        private readonly ILogger _logger;

        private JsonSerializerOptions? _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            AllowTrailingCommas = true,
            IncludeFields = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        public ApiClient(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
            _restClient = BuildClient(_configuration["AutotaskApi:BaseUrl"]);
        }

        public async Task<T> GetAsync<T>(string path)
        {
            var request = BuildRequest(path);
            IRestResponse response = await _restClient.ExecuteAsync(request, _cancellationToken);
            var autotaskApiResponse = JsonSerializer.Deserialize<AutotaskApiResponse<T>>(response.Content, options: _serializerOptions);

            LogRequest(request, response.Content);
            
            if (autotaskApiResponse == null)
            {
                throw new AutotaskApiException<T>(response);
            }
            return autotaskApiResponse.GetEntity();
        }
        
        public async Task<List<T>> QueryAsync<T>(string path, Query query)
        {
            var request = BuildRequest(path, Method.POST, query);
            var response = await _restClient.PostAsync<AutotaskApiResponse<T>>(request, _cancellationToken);
            LogRequest(request, response.ToString());
            
            if (response.HasErrors())
            {
                throw new AutotaskApiException<T>(response);
            }
            
            return response.GetEntities();
        }

        public async Task<bool> PatchAsync<T>(string path, T entity)
        {
            var request = BuildRequest(path, Method.PATCH, entity);
            var response = await _restClient.PutAsync<AutotaskApiResponse<T>>(request, _cancellationToken);
            LogRequest(request, response.ToString());

            if (!response.PatchSuceeded())
            {
                throw new AutotaskApiException<T>(response);
            }
            
            return true;
        }

        public async Task<T> CreateAsync<T>(string path, T entity)
        {
            var request = BuildRequest(path, Method.POST, entity);
            var response = await _restClient.PostAsync<AutotaskApiResponse<T>>(request, _cancellationToken);
            LogRequest(request, response.ToString());

            if (!response.CreateSuceeded())
            {
                throw new AutotaskApiException<T>(response);
            }
            
            return await GetAsync<T>(response.itemId.ToString());
        }

        private IRestClient BuildClient(string baseUrl)
        {
            var restClient = new RestClient(baseUrl);
            restClient.UseSystemTextJson(_serializerOptions);
            restClient.FailOnDeserializationError = true;
            restClient.ThrowOnDeserializationError = true;
            restClient.ThrowOnAnyError = true;
            return restClient;
        }
        
        private RestRequest BuildRequest(string path)
        {
            var request = new RestRequest(path, DataFormat.Json);
            AddHeaders(request);
            return request;
        }

        private RestRequest BuildRequest<T>(string path, Method method, T body)
        {
            var request = BuildRequest(path);
            request.Method = method;
            request.AddJsonBody(body);
            return request;
        }

        private void AddHeaders(IRestRequest request)
        {
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("UserName", _configuration["AutotaskApi:UserName"]);
            request.AddHeader("Secret", _configuration["AutotaskApi:Secret"]);
            request.AddHeader("ApiIntegrationCode", _configuration["AutotaskApi:ApiIntegrationCode"]);
        }
        
        private void LogRequest(IRestRequest request, string entityMessage = null)
        {
            var requestToLog = new
            {
                uri = _restClient.BuildUri(request),
                resource = request.Resource,
                method = request.Method.ToString(),
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                
                body = request.Body
            };

            _logger?.LogInformation($"{Environment.NewLine}>>>{Environment.NewLine}{JsonSerializer.Serialize(requestToLog, _serializerOptions)}");
            if (entityMessage != null)
            {
                _logger?.LogInformation($"{Environment.NewLine}<<<{Environment.NewLine}{entityMessage}");
            }
        }
        
    }
    
    public class PageDetails
    {
        public int count;
        public int requestCount;
        public string prevPageUrl;
        public string nextPageUrl;

        public bool hasNextPage() => nextPageUrl != null;
    }
    
    public class AutotaskApiResponse<T>
    {
        public List<T>? items;
        public T? item;
        public PageDetails? pageDetails;
        public List<string> errors;
        public int? itemId;
        
        public override string ToString()
        {
            if (items != null)
            {
                return $"{typeof(T)}: {items.Count} entities";
            } 
            else if (item != null)
            {
                return item.ToString();
            }
            else if (itemId != null)
            {
                return $"{typeof(T)}: itemID: {itemId}";
            }
            else if (errors != null)
            {
                return $"${typeof(T)}: errors: {ErrorMessage()}";
            }
            return base.ToString();
        }

        public List<T>? GetEntities()
        {
            if (items != null)
            {
                return items;
            }
            else if (item != null)
            {
                return new List<T> { GetEntity() };
            }
            return null;
        }

        public T GetEntity()
        {
            return item;
        }
 
        public bool HasErrors() => errors != null && errors.Count != 0;

        public string ErrorMessage()
        {
            return BaseEntity.DisplayList(errors);
        }

        public bool PatchSuceeded() => itemId != null;

        public bool CreateSuceeded() => itemId != null;
    }
    
    public class AutotaskApiException<T> : Exception
    {
        public AutotaskApiResponse<T>? AutotaskApiResponse;
        
        public AutotaskApiException(string errorMessage) : base(errorMessage)
        {
            
        }

        public AutotaskApiException(AutotaskApiResponse<T> response) : base(response.ErrorMessage())
        {
            AutotaskApiResponse = response;
        }

        public AutotaskApiException(IRestResponse response) : base(response.Content)
        {
            
        }
    }
    
    public class Filter
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("op")] 
        public string ComparisonOperator { get; set; }

        public Filter(string field, string value, string comparisonOperator = "eq")
        {
            Field = field;
            Value = value;
            ComparisonOperator = comparisonOperator;
        }
    }
    
    public class Query
    {
        [JsonPropertyName("Filter")]
        public List<Filter> Filters = new List<Filter>();

        public int? MaxRecords = ApiClient.MAX_PAGE_SIZE;

        public Query(IReadOnlyCollection<Filter> filters, int? maxRecords = null)
        {
            AddFilters(filters);
            if (maxRecords != null)
            {
                MaxRecords = maxRecords;
            }
        }

        public Query(Filter filter, int? maxRecords = null)
        {
            AddFilter(filter);
            
            if (maxRecords != null)
            {
                MaxRecords = maxRecords;
            }
        }

        public Query(int? maxRecords)
        {
            MaxRecords = maxRecords;
        }

        public Query()
        {
        }

        public static Query SimpleQuery(string field, string value, string? comparisonOperator = "eq")
        {
            return new Query(new Filter(field, value, comparisonOperator));
        }

        public void AddFilter(Filter filter)
        {
            Filters.Add(filter);
        }

        public void AddFilters(IEnumerable<Filter> filters)
        {
            this.Filters.AddRange(filters);
        }

        public void RemoveFilter(Filter filter)
        {
            Filters.Remove(filter);
        }
    }
}