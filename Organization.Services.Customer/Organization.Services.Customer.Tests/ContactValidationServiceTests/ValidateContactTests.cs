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

namespace Organization.Services.Customer.Tests.ContactValidationServiceTests
{
    public class ValidateContactTests
    {
        ContactValidationService _contactValidationService;
        Mock<IQueueService> _queueService;

        public ValidateContactTests()
        {
            _queueService = new Mock<IQueueService>();
            _contactValidationService = new ContactValidationService(_queueService.Object);
        }

        [Theory]
        [InlineData("123")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("1 2 3")]
        public async Task ValidatePhoneNumber_ReturnsTrue_WhenAllValid(string phoneNumber)
        {
            var result = await _contactValidationService.ValidateContact(new CustomerContact
            {
                ContactType = Enumerables.CustomerContactType.Phone,
                ContactEntry = phoneNumber
            });

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("AAA")]
        [InlineData("aaa")]
        [InlineData("+")]
        [InlineData("#")]
        [InlineData("1212121212+")]
        public async Task ValidatePhoneNumber_ReturnsFalse_WhenAnyInvalid(string phoneNumber)
        {
            var result = await _contactValidationService.ValidateContact(new CustomerContact
            {
                ContactType = Enumerables.CustomerContactType.Phone,
                ContactEntry = phoneNumber
            });

            result.Should().BeFalse();
        }

        //TODO: improve test data
        [Theory]
        [InlineData("AAA@gmail.com")]
        [InlineData("aaa@yahoo.com")]
        public async Task ValidateEmail_ReturnsTrue_WhenAllValid(string emailAddress)
        {
            var result = await _contactValidationService.ValidateContact(new CustomerContact
            {
                ContactType = Enumerables.CustomerContactType.Email,
                ContactEntry = emailAddress
            });

            result.Should().BeTrue();
        }

        //TODO: improve test data
        [Theory]
        [InlineData("AAA")]
        [InlineData("aaa")]
        [InlineData("+")]
        [InlineData("#")]
        [InlineData("1212121212+")]
        public async Task ValidateEmail_ReturnsFalse_WhenAnyInvalid(string emailAddress)
        {
            var result = await _contactValidationService.ValidateContact(new CustomerContact
            {
                ContactType = Enumerables.CustomerContactType.Email,
                ContactEntry = emailAddress
            });

            result.Should().BeFalse();
        }

        [Fact(Skip ="Queue service needs to be configured in SQS. AWS acc is in sandbox so SES requires 'to' contact to be validated")]
        public async Task ValidateEmail_SendsMessageToQueueService_WhenSuccessful()
        {
            var result = await _contactValidationService.ValidateContact(new CustomerContact
            {
                ContactType = Enumerables.CustomerContactType.Email,
                ContactEntry = "abc@gmail.com",
                RequiresValidation = true
            });

            result.Should().BeTrue();
            _queueService.Verify(x => x.Send(It.IsAny<string>()), Times.Once);
        }
    }
}


