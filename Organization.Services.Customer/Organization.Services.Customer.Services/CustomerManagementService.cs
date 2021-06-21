using Organization.Services.Customer.Enumerables;
using Organization.Services.Customer.Interfaces;
using Organization.Services.Customer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Services.Customer.Services
{
    public class CustomerManagementService : ICustomerManagementService
    {
        IRepositoryService _repositoryService;
        IContactValidationService _contactValidationService;
        public CustomerManagementService(
            IRepositoryService repositoryService,
            IContactValidationService contactValidationService)
        {
            _repositoryService = repositoryService;
            _contactValidationService = contactValidationService;

        }
        public async Task<IResult> Delete(IEnumerable<Guid> customerIds)
        {
            if (!customerIds.Any())
                return BuildCustomerResult(false, string.Empty, "No customerIds provided.");

            var sql = $" where \"ContactId\" in ({string.Join(", ", customerIds)});";

            var result = _repositoryService.Delete<Models.Customer>(sql);
            return BuildCustomerResult(
                await result,
                "Successfully deleted entries from the Customer table",
                "Looks like there was an issue deleting these entries from the database, please contact Application Support.");
        }

        public async Task<IResult> Insert(Models.Customer customer, IEnumerable<CustomerContact> customerContacts)
        {
            if(customer.CustomerId == null) 
                return BuildCustomerResult(false,string.Empty,"Customer requires an Id");

            if(customerContacts.Select(x => x.IsPrimaryContact).Where(x => x == true).Count() != 1)
                return BuildCustomerResult(false, string.Empty, "Customer requires exactly one primary contact");

            //TODO: if entry fails return specific failed contact
            foreach (var contact in customerContacts)
            {
                if (contact.RequiresValidation)
                {
                    if (!await _contactValidationService.ValidateContact(contact))
                        return BuildCustomerResult(false, string.Empty, "Contact details failed validation");
                }
            }

            var customerContactsSql = new StringBuilder();
            foreach (var c in customerContacts)
            {
                customerContactsSql.Append($@"insert into customer_contact values ({string.Join(", ", new List<string>{
                    "'"+c.ContactId.ToString()+"'",
                    "'"+c.CustomerId.ToString()+"'",
                    ""+(int)c.ContactType,
                    "'"+c.ContactEntry+"'",
                    c.IsPrimaryContact.ToString(),
                    c.RequiresValidation.ToString()
                })});");
            }

            var customerSql = $@"insert into customer values ({string.Join(", ", new List<string> {
                    "'"+customer.CustomerId.ToString()+"'",
                    ""+(int)customer.Status,
                    "'"+ToPGDate(DateTime.UtcNow)+"'",
                    "'"+ToPGDate(DateTime.UtcNow)+"'",
                    "'"+customer.FirstName+"'",
                    "'"+customer.LastName+"'",
                    "'"+customer.PreferredName+"'",
                    "'"+ToPGDate(customer.DateOfBirth)+"'"
                })});";

            //TODO: should push contact details simultaneously
            var contactResult = await _repositoryService.ExecuteAsync<CustomerContact>(customerContactsSql.ToString());
            var customerResult = await _repositoryService.ExecuteAsync<Models.Customer>(customerSql);

            return BuildCustomerResult(
                contactResult && customerResult,
                "Customer added successfully",
                "Customer could not be added");
        }

        private string ToPGDate(DateTime date)
        {
            return $"{date.Year}-{date.Month}-{date.Day}";
        }

        public async Task<IResult> SelectAll(CustomerStatus? status)
        {
            //TODO: should return ids only to reduce load from DB
            var sql = string.Empty;
            if (status != null) sql = $" WHERE \"Status\" = {(int)status}";
            
            var customers = await _repositoryService.SelectAll<Models.Customer>(sql);
            return BuildCustomerResult(
                customers.Any(), 
                "Customers retrieved successfully", 
                "No customers found with status provided",
                customers.Select(x => x.CustomerId).OrderBy(x => x));
        }

        public async Task<IResult> SelectSingle(Guid customerId)
        {
            var sql = $" WHERE \"CustomerId\" = '{customerId}'";
            var customer = await _repositoryService.SelectSingle<Models.Customer>(sql);
            var contacts = await _repositoryService.SelectAll<CustomerContact>(sql);

            return BuildCustomerResult(
                customer != null,
                "Customer retrieved successfully",
                "No customer found with id provided",
                new { 
                    customer,
                    contacts
                });
        }

        public async Task<IResult> Update(Models.Customer customer)
        {
            //TODO: handle contact data
            if(customer.CustomerId == null) return BuildCustomerResult(false, string.Empty, "CustomerId not provided");

            var existing = await _repositoryService.SelectSingle<Models.Customer>($" WHERE \"CustomerId\" = '{customer.CustomerId}'");
            if (existing == null) return BuildCustomerResult(false, string.Empty, "Customer could not be found, please try adding a new customer");

            //these sql queries really need to be replaced... 
            var customerSql = $"update customer set \"Status\" = {(int)customer.Status}, \"UpdatedAt\" = '{ToPGDate(DateTime.UtcNow)}' where \"CustomerId\" = '{customer.CustomerId}' and \"UpdatedAt\" = '{ToPGDate(existing.UpdatedAt)}';";
            var customerResult = await _repositoryService.ExecuteAsync<Models.Customer>(customerSql);

            return BuildCustomerResult(
                customerResult,
                "Customer updated successfully",
                "Customer could not be updated");
        }

        private IResult BuildCustomerResult(
            bool success,
            string successMessage,
            string failureMessage,
            object payload = null)
        {
            return success switch
            {
                true => new CustomerSuccessResult
                {
                    Message = successMessage,
                    Payload = payload
                },
                false => new CustomerFailureResult
                {
                    Message = failureMessage
                }
            };
        }
    }
}
