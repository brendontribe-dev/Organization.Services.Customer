using Organization.Services.Customer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Services.Customer.Interfaces
{
    public interface ICustomerNoteManagementService
    {
        public Task<IResult> Delete(IEnumerable<Guid> customerNoteIds);
        public Task<IResult> Insert(CustomerNote customerNotes); 
        public Task<IResult> SelectAll(Guid customerId);
        public Task<IResult> Update(CustomerNote customerNote);
    }
}
