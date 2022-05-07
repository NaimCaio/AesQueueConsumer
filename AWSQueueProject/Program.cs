using Amazon.SQS;
using AWSQueueProject.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AWSQueueProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var sqsClient = new AmazonSQSClient(Amazon.RegionEndpoint.USEast1);
            var sqsService = new SQSQueueService();
            var queueUrl = sqsService.CreateOrConectQueue(sqsClient, "MySQS", "30");
            sqsService.ReceiveMessages(queueUrl, sqsClient);
        }
    }
}
