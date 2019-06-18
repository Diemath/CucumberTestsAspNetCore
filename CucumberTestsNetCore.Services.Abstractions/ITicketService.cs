using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CucumberTestsNetCore.Services.Abstractions
{
    public interface ITicketService
    {
        Task DeleteOldTicketsAsync();
    }
}
