using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Organization.Services.Customer.Enumerables;
using Organization.Services.Customer.Interfaces;
using Organization.Services.Customer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Organization.Services.Customer.Services
{
    public class ContactValidationService : IContactValidationService
    {
        private readonly IQueueService _queueService;

        public ContactValidationService(IQueueService queueService)
        {
            _queueService = queueService;
        }

        public async Task<bool> ValidateContact(CustomerContact contact)
        {
            return contact.ContactType switch
            {
                CustomerContactType.Email => await ValidateEmail(contact),
                CustomerContactType.Phone => ValidatePhoneNumber(contact.ContactEntry),
                _ => throw new ArgumentException($"CustomerContactType '{contact.ContactType}' was not recognised.", nameof(contact.ContactType))
            };
        }

        private async Task<bool> ValidateEmail(CustomerContact contact)
        {
            if (!ValidateEmailFormat(contact.ContactEntry)) return false;
            //TODO: Emails can not be sent to unverified accounts from a sandbox account in aws
            //The purpose of the queue was to send a welcome email to the primary email contact
            //if (contact.IsPrimaryContact) await _queueService.Send(contact.ContactEntry);
            
            return true;
        }

        private bool ValidateEmailFormat(string email)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();

                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            //TODO: make the regex stricter on count of numbers
            //TODO: allow "+", "#" symbol etc.
            return phoneNumber.All(x => char.IsDigit(x) || char.IsWhiteSpace(x));
        }
    }
}