using System;
using System.Collections.Generic;
using System.Text;

namespace CucumberTestsNetCore.Services.Abstractions
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}
