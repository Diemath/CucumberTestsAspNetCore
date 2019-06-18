using CucumberTestsNetCore.Services.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace CucumberTestsNetCore.Services.XUnit.Features.StepDefinitions
{
    [Binding]
    public class OldTicketsDeletingSteps
    {
        public OldTicketsDeletingSteps()
        {
            SaveDeletedTickets();
        }

        private Mock<IDateTime> _mockDateTime { get; set; } = new Mock<IDateTime>();
        private Mock<IOptions<TicketsOptions>> _mockTicketsOptions { get; set; } = new Mock<IOptions<TicketsOptions>>();
        private Mock<ITicketRepository> _mockTicketRepository { get; set; } = new Mock<ITicketRepository>();
        private IList<Ticket> _mockTickets { get; set; } = new List<Ticket>();
        private IEnumerable<Ticket> _deletedTickets { get; set; }

        [Given(@"today is ""(.*)""")]
        public void GivenTodayIs(DateTime today)
        {
            _mockDateTime.SetupGet(d => d.Now).Returns(today);
        }
        
        [Given(@"configuration has value for days limit before deleting of tickets - ""(.*)""")]
        public void GivenConfigurationHasValueForDaysLimitBeforeDeletingOfTickets_(int daysBeforeDeleting)
        {
            _mockTicketsOptions.SetupGet(o => o.Value).Returns(
                new TicketsOptions { DaysBeforeDeleting = daysBeforeDeleting });
        }
        
        [Given(@"three tickets exist\. First ticket was created at ""(.*)""")]
        public void GivenThreeTicketsExist_FirstTicketWasCreatedAt(DateTime created)
        {
            _mockTickets.Add(new Ticket { CreatedUtc = created });
        }
        
        [Given(@"second ticket was created at ""(.*)""")]
        public void GivenSecondTicketWasCreatedAt(DateTime created)
        {
            _mockTickets.Add(new Ticket { CreatedUtc = created });
        }
        
        [Given(@"third ticket was created at ""(.*)""")]
        public void GivenThirdTicketWasCreatedAt(DateTime created)
        {
            _mockTickets.Add(new Ticket { CreatedUtc = created });
        }
        
        [When(@"old tickets deleting is called")]
        public async Task WhenOldTicketsDeletingIsCalled()
        {
            await new TicketService(
                _mockTicketsOptions.Object,
                _mockDateTime.Object,
                _mockTicketRepository.Object)
                .DeleteOldTicketsAsync();
        }
        
        [Then(@"first ticket created at ""(.*)"" wont be deleted")]
        public void ThenFirstTicketCreatedAtWontBeDeleted(DateTime created)
        {
            Assert.DoesNotContain(_deletedTickets, t => t.CreatedUtc == created);
        }
        
        [Then(@"second ticket created at ""(.*)"" will be deleted")]
        public void ThenSecondTicketCreatedAtWillBeDeleted(DateTime created)
        {
            Assert.Contains(_deletedTickets, t => t.CreatedUtc == created);
        }
        
        [Then(@"third ticket created at ""(.*)"" will be deleted")]
        public void ThenThirdTicketCreatedAtWillBeDeleted(DateTime created)
        {
            Assert.Contains(_deletedTickets, t => t.CreatedUtc == created);
        }

        private void SaveDeletedTickets()
        {
            _mockTicketRepository
                .Setup(r => r.DeleteRangeAsync(It.IsAny<Expression<Func<Ticket, bool>>>()))
                .Callback((Expression<Func<Ticket, bool>> p) => _deletedTickets = _mockTickets.Where(p.Compile()))
                .Returns(Task.CompletedTask);
        }
    }

    public class TicketsOptions
    {
        public int DaysBeforeDeleting { get; set; }
    }

    public class TicketService : ITicketService
    {
        private readonly TicketsOptions _options;
        private readonly IDateTime _dateTime;
        private readonly ITicketRepository _ticketRepository;

        public TicketService(
            IOptions<TicketsOptions> options, 
            IDateTime dateTime, 
            ITicketRepository ticketRepository)
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
