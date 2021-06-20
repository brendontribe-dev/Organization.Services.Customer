using Organization.Services.Customer.Enumerables;
using Organization.Services.Customer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Services.Customer.Interfaces
{
    public interface ICustomerManagementService
    {
        public Task<IResult> Delete(IEnumerable<Guid> customerIds);
        public Task<IResult> Insert(Models.Customer customer, IEnumerable<CustomerContact> customerContacts);
        public Task<IResult> SelectAll(CustomerStatus? status);
        public Task<IResult> SelectSingle(Guid customerId);
        public Task<IResult> Update(Models.Customer customer);
    }
}
