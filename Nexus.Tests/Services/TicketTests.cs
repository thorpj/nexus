using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using Xunit;
using Nexus.Server.Services.Autotask;
using Nexus.Shared.Models.Autotask;
// using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using Xunit.Abstractions;

namespace Nexus.Tests.Controllers.Autotask
{
    public class TicketTests : TestsBase
    {
        private ApiClient _client;

        public TicketTests(ITestOutputHelper output) : base(output)
        {
            _client = new ApiClient(Configuration, Logger);
        }

        [Fact]
        public void TestSerializeQuery()
        {
            // var query = new Query(new Filter("id", "123"));
            var query = new Query(new List<Filter> {new Filter("id", "68275"), new Filter("id", "68275")});
            var json = JsonConvert.SerializeObject(query);
            Logger?.LogInformation(json);
        }


        [Fact]
        public async void TestSerializeEntity()
        {
            var ticket = await new ApiClient(Configuration, Logger).GetAsync<Ticket>("Tickets/68275");
            Logger?.LogInformation(JsonConvert.SerializeObject(ticket));
        }


        [Fact]
        public async void TestGetEntity()
        {
            var ticket = await _client.GetAsync<Ticket>("Tickets/68275");
            Assert.NotNull(ticket);
            Logger?.LogInformation(ticket.ToString());
        }

        [Fact]
        public async void TestGetEntities()
        {
            var query = Query.SimpleQuery("id", "68275");
            var tickets = await _client.QueryAsync<Ticket>("Tickets/query", query);
            Assert.NotNull(tickets);
            Assert.NotEmpty(tickets);
            Logger?.LogInformation(tickets[0].ToString());
        }

        [Fact]
        public async void TestQueryError()
        {
            var query = Query.SimpleQuery("id", "68275", "some\\in\nvalidoperator\\");
            await Assert.ThrowsAsync<AutotaskApiException<Ticket>>(async () =>
                await _client.QueryAsync<Ticket>("Tickets", query));
        }

        [Fact]
        public async void TestPatchError()
        {
            await Assert.ThrowsAsync<AutotaskApiException<Ticket>>(
                async () => await _client.PatchAsync("Tickets", new Ticket()));
        }

        [Fact]
        public async void TestUpdateEntity()
        {
            var ticket = await _client.GetAsync<Ticket>("Tickets/68275");
            ticket.Title = $"Updated Title {DateTime.Now.ToString("dd/MM/yyyy h:mm tt")}";
            await _client.PatchAsync("Tickets", ticket);
        }
    }
}