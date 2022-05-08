using Amazon.SQS;
using AWSQueueProject.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;

namespace AWSQueueProject
{
    class Program
    {
        static void Main(string[] args)
        {
            

            string userKey = Environment.GetEnvironmentVariable("key");
            string userSecret = Environment.GetEnvironmentVariable("secret");
            string queueName = Environment.GetEnvironmentVariable("queuename");
            string timeout = Environment.GetEnvironmentVariable("timeoutqueue");
            var sqsClient = new AmazonSQSClient(userKey, userSecret,Amazon.RegionEndpoint.USEast1);
            var sqsService = new SQSQueueService();
            var queueUrl = sqsService.CreateOrConectQueue(sqsClient, queueName, timeout);
            sqsService.ReceiveMessages(queueUrl, sqsClient);
        }
    }
}
