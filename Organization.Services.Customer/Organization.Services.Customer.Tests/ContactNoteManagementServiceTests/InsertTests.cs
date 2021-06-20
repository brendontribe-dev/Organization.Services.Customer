using FluentAssertions;
using Moq;
using Organization.Services.Customer.Interfaces;
using Organization.Services.Customer.Models;
using Organization.Services.Customer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Organization.Services.Customer.Tests.ContactNoteManagementServiceTests
{
    public class InsertTests
    {
        CustomerNoteManagementService _service;
        Mock<IRepositoryService> _repositoryService;
        Mock<ICustomerManagementService> _customerManagementService;
        public InsertTests()
        {
            _repositoryService = new Mock<IRepositoryService>();
            _service = new CustomerNoteManagementService(_repositoryService.Object, _customerManagementService.Object);
        }

        [Fact]
        public async Task Insert_ReturnsSuccessMessage_WhenInsertionSucceeds()
        {
            //TODO:_repositoryService.Setup(x => x.Insert<CustomerNote>(It.IsAny<IEnumerable<CustomerNote>>())).ReturnsAsync(true);
            _repositoryService.Setup(x => x.ExecuteAsync<CustomerNote>(It.IsAny<string>())).ReturnsAsync(true);
            
            var result = await _service.Insert(new CustomerNote());

            result.GetType().Should().Be(typeof(CustomerNoteSuccessResult));
            var successResult = (CustomerNoteSuccessResult)result;

            successResult.Payload.Should().BeNull();
            successResult.Message.Should()
                .Be("Successfully deleted all entries from the CustomerNotes table");
        }

        [Fact]
        public async Task Insert_ReturnsFailureMessage_WhenInsertionFails()
        {
            //TODO:_repositoryService.Setup(x => x.Insert<CustomerNote>(It.IsAny<IEnumerable<CustomerNote>>())).ReturnsAsync(false);
            _repositoryService.Setup(x => x.ExecuteAsync<CustomerNote>(It.IsAny<string>())).ReturnsAsync(false);
            
            var result = await _service.Insert(CustomerNote());

            result.GetType().Should().Be(typeof(CustomerNoteFailureResult));
            var failureResult = (CustomerNoteFailureResult)result;

            failureResult.Message.Should()
                .Be("Can not find CustomerId in database.");
        }

        //TODO: cant find customerId
        //TODO: no notes in enumerable
        //TODO: multiple customerIds
    }
}
