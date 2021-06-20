using Organization.Services.Customer.Interfaces;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Organization.Services.Customer.Enumerables;
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
    public class QueueService : IQueueService
    {
        //TODO: set up production AWS account to receive SQS events
        //send an email using SES and lambda

        private readonly IAmazonSQS _sqs;
        public QueueService()
        {
            _sqs = new AmazonSQSClient(
                new BasicAWSCredentials("", ""),
                RegionEndpoint.APSoutheast2);
        }

        public async Task Send(string message)
        {
            try
            {
                await _sqs.SendMessageAsync(new SendMessageRequest
                {
                    QueueUrl = "",
                    MessageBody = message
                });
            }
            catch
            {
                //TODO: log messsage
            }
            
        }
    }
}
