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
    [Route("[controller]")]
    public class CustomerNoteController : ControllerBase
    {
        ICustomerNoteManagementService _customerNoteManagementService;

        public CustomerNoteController(ICustomerNoteManagementService customerNoteManagementService)
        {
            _customerNoteManagementService = customerNoteManagementService;
        }

        [Route("/customer-note/insert"), HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<IResult>> Insert(CustomerNote customerNote)
        {
            var result = await _customerNoteManagementService.Insert(customerNote);

            //TODO: other error codes
            switch (result)
            {
                case CustomerSuccessResult _:
                    return Ok(result);
                default:
                    return BadRequest(result);
            }
        }

        [Route("/customer-note/update"), HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<IResult>> Update(CustomerNote customerNote)
        {
            var result = await _customerNoteManagementService.Update(customerNote);

            //TODO: other error codes
            switch (result)
            {
                case CustomerSuccessResult _:
                    return Ok(result);
                default:
                    return BadRequest(result);
            }
        }

        [Route("/customer-note/select-all"), HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IResult>> SelectAll([FromQuery] Guid customerId)
        {
            var result = await _customerNoteManagementService.SelectAll(customerId);

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
