using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CucumberTestsNetCore.Services.Abstractions
{
    public interface ITicketRepository
    {
        Task DeleteRangeAsync(Expression<Func<Ticket, bool>> predicate);
    }
}
