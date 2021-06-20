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
    public class DeleteTests
    {
        CustomerNoteManagementService _service;
        Mock<IRepositoryService> _repositoryService;
        Mock<ICustomerManagementService> _customerManagementService;
        public DeleteTests()
        {
            _repositoryService = new Mock<IRepositoryService>();
            _service = new CustomerNoteManagementService(_repositoryService.Object, _customerManagementService.Object);
        }

        [Fact]
        public async Task Delete_ReturnsSuccessMessage_WhenDeletionSucceeds()
        {
            //TODO: _repositoryService.Setup(x => x.Delete<CustomerNote>(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(true);
            _repositoryService.Setup(x => x.Delete<CustomerNote>(It.IsAny<string>())).ReturnsAsync(true);
            var result = await _service.Delete(new List<Guid>
            {
                Guid.NewGuid()
            });

            result.GetType().Should().Be(typeof(CustomerNoteSuccessResult));
            var successResult = (CustomerNoteSuccessResult)result;

            successResult.Payload.Should().BeNull();
            successResult.Message.Should()
                .Be("Successfully deleted all entries from the CustomerNotes table");
        }

        [Fact]
        public async Task Delete_ReturnsFailureMessage_WhenEmptyEnumerableProvided()
        {
            var result = await _service.Delete(new List<Guid> { });

            result.GetType().Should().Be(typeof(CustomerNoteFailureResult));
            var failureResult = (CustomerNoteFailureResult)result;

            failureResult.Message.Should()
                .Be("No noteIds provided.");
        }

        [Fact]
        public async Task Delete_ReturnsFailureMessage_WhenDeletionFails()
        {
            //TODO: _repositoryService.Setup(x => x.Delete<CustomerNote>(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(false);
            _repositoryService.Setup(x => x.Delete<CustomerNote>(It.IsAny<string>())).ReturnsAsync(false);
            var result = await _service.Delete(new List<Guid>
            {
                Guid.NewGuid()
            });

            result.GetType().Should().Be(typeof(CustomerNoteFailureResult));
            var failureResult = (CustomerNoteFailureResult)result;

            failureResult.Message.Should()
                .Be("Looks like there was an issue deleting these entries from the database, please contact Application Support.");
        }
    }
}