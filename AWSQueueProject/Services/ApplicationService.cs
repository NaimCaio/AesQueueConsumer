using Amazon.SQS;
using AWSQueueProject.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSQueueProject.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ISQSQueueService _sqsService;
        public ApplicationService(ISQSQueueService sqsService)
        {
            _sqsService = sqsService;   
        }
        public void startService()
        {
            string userKey = Environment.GetEnvironmentVariable("key");
            string userSecret = Environment.GetEnvironmentVariable("secret");
            string queueName = Environment.GetEnvironmentVariable("queuename");
            string timeout = Environment.GetEnvironmentVariable("timeoutqueue");
            var sqsClient = new AmazonSQSClient(userKey, userSecret, Amazon.RegionEndpoint.USEast1);

            var queueUrl = _sqsService.CreateOrConectQueue(sqsClient, queueName, timeout);
            
            _sqsService.ReceiveMessages(queueUrl, sqsClient);
        }
    }

    
}
