using CucumberTestsNetCore.Services.Abstractions;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace CucumberTestsNetCore.Services
{
    public class TicketService : ITicketService
    {
        private readonly TicketsOptions _options;
        private readonly IDateTime _dateTime;
        private readonly ITicketRepository _ticketRepository;

        public TicketService(IOptions<TicketsOptions> options, IDateTime dateTime, ITicketRepository ticketRepository)
        {
            _options = options.Value;
            _dateTime = dateTime;
            _ticketRepository = ticketRepository;
        }

        public async Task DeleteOldTicketsAsync()
        {
            await _ticketRepository.DeleteRangeAsync(t => t.CreatedUtc.Date.AddDays(_options.DaysBeforeDeleting) < _dateTime.Now.Date);
        }
    }
}
