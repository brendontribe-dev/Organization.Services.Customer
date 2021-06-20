using Organization.Services.Customer.Interfaces;
using Organization.Services.Customer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Services.Customer.Services
{
    public class CustomerNoteManagementService : ICustomerNoteManagementService
    {
        IRepositoryService _repositoryService;
        ICustomerManagementService _customerManagementService;

        public CustomerNoteManagementService(
            IRepositoryService repositoryService,
            ICustomerManagementService customerManagementService)
        {
            _customerManagementService = customerManagementService;
            _repositoryService = repositoryService;
        }

        public async Task<IResult> Delete(IEnumerable<Guid> customerNoteIds)
        {
            if (!customerNoteIds.Any())
                return BuildCustomerNoteResult(false, string.Empty, "No noteIds provided.");

            var sql = $" where \"ContactId\" in ({string.Join(", ", customerNoteIds)});";

            var result = _repositoryService.Delete<CustomerNote>(sql);
            return BuildCustomerNoteResult(
                await result,
                "Successfully deleted all entries from the CustomerNotes table",
                "Looks like there was an issue deleting these entries from the database, please contact Application Support.");
        }

        public async Task<IResult> Insert(CustomerNote customerNote)
        {
            /* TODO: update multiple notes at once
            if (!customerNotes.Any())
                return BuildCustomerNoteResult(false, string.Empty, "No notes provided.");

            if (customerNotes.Select(x => x.CustomerId).Count() != 1)
                return BuildCustomerNoteResult(false, string.Empty, "Notes must apply to exactly one customer");
            */

            if (await _customerManagementService.SelectSingle(customerNote.CustomerId) == null)
                return BuildCustomerNoteResult(false, string.Empty, "Can not find CustomerId in database.");

            var customerNotesSql = $@"insert into customer_note values ({string.Join(", ", new List<string> {
                    "'"+customerNote.NoteId.ToString()+"'",
                    "'"+customerNote.CustomerId.ToString()+"'",
                    "'"+ToPGDate(customerNote.AuthoredDateTime)+"'",
                    "'"+customerNote.Contents+"'",
                    ""+(int)customerNote.Status,
                })});";

            var result = _repositoryService.ExecuteAsync<CustomerNote>(customerNotesSql);
            return BuildCustomerNoteResult(
                await result,
                "Successfully inserted records into the CustomerNotes table",
                "Looks like there was an issue inerting entries into the database, please contact Application Support.");       
        }

        private string ToPGDate(DateTime date)
        {
            return $"{date.Year}-{date.Month}-{date.Day}";
        }

        //TODO: If customer doesn't exist vs when they have no notes associated
        public async Task<IResult> SelectAll(Guid customerId)
        {
            //TODO: This is the worst...
            //Repository should be any store e.g. a DB, S3, a noSQL DB, etc this sql query requirement is an example of what not to do...
            var sql = $" WHERE \"CustomerId\" = '{customerId}'";
            var result = await _repositoryService.SelectAll<CustomerNote>(sql);
            //TODO: may be valid to return zero notes in which case returning a failure would be unexpected, 
            //this can be changed when the DB response is improved
            return BuildCustomerNoteResult(
                result.Any(),
                "Successfully returned results",
                "No results found",
                result
                );
        }

        public async Task<IResult> Update(CustomerNote customerNotes)
        {
            /*TODO 
            if (!customerNotes.Any())
                return BuildCustomerNoteResult(false, string.Empty, "No notes provided.");

            if (customerNotes.Select(x => x.CustomerId).Count() != 1)
                return BuildCustomerNoteResult(false, string.Empty, "Notes must apply to exactly one customer");
            */

            var customerNotesSql = $"update customer_note set \"Contents\" = '{customerNotes.Contents}' where \"NoteId\" = '{customerNotes.NoteId}';";

            var result = _repositoryService.ExecuteAsync<CustomerNote>(customerNotesSql);
            return BuildCustomerNoteResult(
                await result,
                "Successfully updated records in the CustomerNotes table",
                "Looks like there was an issue updating entries in the database, please contact Application Support.");
        }

        private IResult BuildCustomerNoteResult(
            bool success, 
            string successMessage, 
            string failureMessage,
            object payload = null)
        {
            return success switch
            {
                true => new CustomerNoteSuccessResult
                {
                    Message = successMessage,
                    Payload = payload
                },
                false => new CustomerNoteFailureResult
                {
                    Message = failureMessage
                }
            };
        }
    }
}
