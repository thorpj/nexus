using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nexus.Shared.Models.Autotask;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nexus.Server.Services.Autotask;

namespace Nexus.Server.Controllers.Autotask
{
    [ApiController]
    [Route("api/autotask/[controller]")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new [] {"*"})]
    public class TicketsController : ControllerBase
    {
        private readonly ApiClient _apiClient;
        private readonly ILogger<TicketsController> _logger;
        private readonly IConfiguration _configuration;

        
        public TicketsController(ILogger<TicketsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _apiClient = new ApiClient(_configuration, _logger);
        }
        
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(string id)
        {
            _logger.LogInformation("Fetching Tickets/{Id} from AT API", id);
            var ticket = await _apiClient.GetAsync<Ticket>($"Tickets/{id}");
            return ticket;
        }

        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> Index()
        {
            var query = new Query(50);
            query.AddFilter(new Filter("id", "0", "gt"));
            var tickets =  await _apiClient.QueryAsync<Ticket>("Tickets/query", query);
            return tickets;
        }
    }
}