using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Services.Customer.Interfaces
{
    public interface IQueueService
    {
        public Task Send(string message);
    }
}
