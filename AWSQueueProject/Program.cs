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
            var sqsClient = new AmazonSQSClient(userKey, userSecret,Amazon.RegionEndpoint.USEast1);
            var sqsService = new SQSQueueService();
            var queueUrl = sqsService.CreateOrConectQueue(sqsClient, "MySQS", "30");
            sqsService.ReceiveMessages(queueUrl, sqsClient);
        }
    }
}
