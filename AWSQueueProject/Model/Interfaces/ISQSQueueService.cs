using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSQueueProject.Model.Interfaces
{
    public interface ISQSQueueService
    {
        string CreateOrConectQueue(AmazonSQSClient sqsClient, string queueName, string timeout);
        void ReceiveMessages(string queueUrl, AmazonSQSClient sqsClient);

        int MessagensInQueue(string queueUrl, AmazonSQSClient sqsClient);
        void UpdateDabase(NotificationDTO obj);
        void DeleteMessage(Message obj, string queueUrl, AmazonSQSClient sqsClient);




    }
}
