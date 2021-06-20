using FluentAssertions;
using Moq;
using Organization.Services.Customer.Interfaces;
using Organization.Services.Customer.Models;
using Organization.Services.Customer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Organization.Services.Customer.Tests.ContactManagementServiceTests
{
    public class DeleteTests
    {
        CustomerManagementService _service;
        Mock<IRepositoryService> _repositoryService;
        Mock<IContactValidationService> _contactValidationService;
        public DeleteTests()
        {
            _repositoryService = new Mock<IRepositoryService>();
            _contactValidationService = new Mock<IContactValidationService>();
            _service = new CustomerManagementService(_repositoryService.Object, _contactValidationService.Object);
        }

        [Fact]
        public async Task Delete_ReturnsSuccessMessage_WhenDeletionSucceeds()
        {
            //TODO: _repositoryService.Setup(x => x.Delete<Models.Customer>(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(true);
            _repositoryService.Setup(x => x.Delete<Models.Customer>(It.IsAny<string>())).ReturnsAsync(true);
            var result = await _service.Delete(new List<Guid>
            {
                Guid.NewGuid()
            });

            result.GetType().Should().Be(typeof(CustomerSuccessResult));
            var successResult = (CustomerSuccessResult)result;

            successResult.Payload.Should().BeNull();
            successResult.Message.Should()
                .Be("Successfully deleted entries from the Customer table");
        }

        [Fact]
        public async Task Delete_ReturnsFailureMessage_WhenEmptyEnumerableProvided()
        {
            var result = await _service.Delete(new List<Guid> { });

            result.GetType().Should().Be(typeof(CustomerFailureResult));
            var failureResult = (CustomerFailureResult)result;

            failureResult.Message.Should()
                .Be("No customerIds provided.");
        }

        [Fact]
        public async Task Delete_ReturnsFailureMessage_WhenDeletionFails()
        {
            //TODO: _repositoryService.Setup(x => x.Delete<Models.Customer>(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(false);
            _repositoryService.Setup(x => x.Delete<Models.Customer>(It.IsAny<string>())).ReturnsAsync(false);
            var result = await _service.Delete(new List<Guid>
            {
                Guid.NewGuid()
            });

            result.GetType().Should().Be(typeof(CustomerFailureResult));
            var failureResult = (CustomerFailureResult)result;

            failureResult.Message.Should()
                .Be("Looks like there was an issue deleting these entries from the database, please contact Application Support.");
        }
    }
}