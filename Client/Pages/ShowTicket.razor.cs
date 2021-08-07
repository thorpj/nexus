using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Nexus.Shared.Models.Autotask;

namespace Nexus.Client.Pages
{
    public partial class ShowTicket : ComponentBase
    {
        [Inject]
        HttpClient Http { get; set; }

        private IEnumerable<Ticket> _tickets;
        private Ticket _ticket;

        protected override async Task OnInitializedAsync()
        {
            // _tickets = await Http.GetFromJsonAsync<IEnumerable<Ticket>>("api/autotask/tickets");
            _ticket = await Http.GetFromJsonAsync<Ticket>("api/autotask/tickets/68275");
        }
    }
}