using Organization.Services.Customer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Services.Customer.Interfaces
{
    public interface IContactValidationService
    {
        public Task<bool> ValidateContact(CustomerContact contact);
    }
}
