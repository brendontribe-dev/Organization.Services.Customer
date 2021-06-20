using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Organization.Services.Customer.Interfaces;
using Organization.Services.Customer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organization.Services.Customer.Controllers.Host
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ICustomerManagementService _customerManagementService;
        public CustomerController(ICustomerManagementService customerManagementService)
        {
            _customerManagementService = customerManagementService;
        }

        [Route("/customer/insert"),HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<IResult>> Insert(CustomerDto customer)
        {
            var result = await _customerManagementService.Insert(customer.Customer, customer.Contacts);
            
            //TODO: other error codes
            switch (result)
            {
                case CustomerSuccessResult _:
                    return Ok(result);
                default:
                    return BadRequest(result);
            }
        }

        [Route("/customer/update"), HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<IResult>> Update(CustomerDto customer)
        {
            var result = await _customerManagementService.Update(customer.Customer);

            //TODO: other error codes
            switch (result)
            {
                case CustomerSuccessResult _:
                    return Ok(result);
                default:
                    return BadRequest(result);
            }
        }

        [Route("/customer/select-all"), HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IResult>> SelectAll([FromQuery] int status)
        {
            var result = await _customerManagementService.SelectAll((Enumerables.CustomerStatus) status);

            //TODO: other error codes
            switch (result)
            {
                case CustomerSuccessResult _:
                    return Ok(result);
                default:
                    return BadRequest(result);
            }
        }

        [Route("/customer/select"), HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IResult>> Select([FromQuery] Guid contactId)
        {
            var result = await _customerManagementService.SelectSingle(contactId);

            //TODO: other error codes
            switch (result)
            {
                case CustomerSuccessResult _:
                    return Ok(result);
                default:
                    return BadRequest(result);
            }
        }
    }
}

public class CustomerDto
{
    public Customer Customer { get; set; }
    public IEnumerable<CustomerContact> Contacts { get; set; }
}