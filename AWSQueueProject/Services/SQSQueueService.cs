﻿using Amazon.SQS;
using Amazon.SQS.Model;
using AWSQueueProject.Model;
using AWSQueueProject.Model.Context;
using AWSQueueProject.Model.Interfaces;
using AWSQueueProject.Model.Repositorys;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSQueueProject
{
    public class SQSQueueService: ISQSQueueService
    {
        public readonly IGenericRepository<File> _filesRepository;
        public SQSQueueService(IGenericRepository<File> filesRepository)
        {
            _filesRepository = filesRepository; 
        }
        public  string CreateOrConectQueue( AmazonSQSClient sqsClient, string queueName, string timeout)
        {
            var queueRequest = new CreateQueueRequest();
            queueRequest.QueueName = queueName;

            var attrs = new Dictionary<string, string>();
            attrs.Add(QueueAttributeName.VisibilityTimeout, timeout);
            queueRequest.Attributes = attrs;

            try
            {
                var queueCriationResponse = Task.Run(async () => await sqsClient.CreateQueueAsync(queueRequest)).Result;
                return queueCriationResponse.QueueUrl;
            }
            catch (Exception )
            {

                throw new Exception("Erro ao criar ou conectar a fila");
            }


        }

        public void ReceiveMessages(string queueUrl, AmazonSQSClient sqsClient)
        {
            var receiveMessageRquest = new ReceiveMessageRequest();
            receiveMessageRquest.QueueUrl = queueUrl;

            var counter = 0;
            var length = MessagensInQueue(queueUrl, sqsClient);
            while (counter<length)
            {
                Console.WriteLine("Obtendo mensagem da fila");
                var receivaedMessageResponse = Task.Run(async () => await sqsClient.ReceiveMessageAsync(receiveMessageRquest)).Result;
                
                if (receivaedMessageResponse.HttpStatusCode== System.Net.HttpStatusCode.OK && receivaedMessageResponse.Messages.Count>0)
                {
                    Console.WriteLine("Mensagem lida com sucesso");
                    var message = receivaedMessageResponse.Messages[0];
                    var obj = JsonConvert.DeserializeObject<NotificationDTO>(message.Body);
                    
                    //Só atualiza a base para mensagens de notificação
                    if (obj.Records != null)
                    {
                        UpdateDabase(obj);

                    }
                    counter++;
                    //Caso queira deletar a mensagem da fila depois de ler ela só precisaria descomentar a linha abaixo
                    //Fiquei na dúvida se faz parte do objetivo perder as informações das notificações ou não
                    //DeleteMessage(message, queueUrl, sqsClient);



                }
                else
                {
                    Console.WriteLine("Erro ao acessar fila");
                    throw new Exception("Erro ao acessar fila");
                }
                

            }
        }

        public int MessagensInQueue(string queueUrl, AmazonSQSClient sqsClient)
        {
            var messagesQtd = 0;

            var attReq = new GetQueueAttributesRequest();
            attReq.QueueUrl = queueUrl;
            attReq.AttributeNames.Add("ApproximateNumberOfMessages"); 

             var response = Task.Run(async () => await sqsClient.GetQueueAttributesAsync(attReq)).Result;

            messagesQtd = response.ApproximateNumberOfMessages;

            return messagesQtd;
        }

        public  void UpdateDabase(NotificationDTO obj)
        {
            var AWSfile = new File()
            {
                filename = obj.Records[0].s3.Object.key,
                filesize = obj.Records[0].s3.Object.size !=null? long.Parse(obj.Records[0].s3.Object.size):0,
                lastmodified = DateTime.Parse(obj.Records[0].eventTime)

            };
            var databaseFile = _filesRepository.FirstOrDeafault(f => f.filename == AWSfile.filename);
            if (databaseFile==null)
            {
                Console.WriteLine("Adicionando novo arquivo");
                try
                {
                    _filesRepository.Add(AWSfile);
                    _filesRepository.Save();
                }
                catch (Exception)
                {

                    throw new Exception("Falha ao adicionar novo arquivo");
                }
                
            }
            else
            {
                Console.WriteLine("Arquivo {0} já existente, valores serão atualizados ", databaseFile.filename);
                if (AWSfile.lastmodified >= databaseFile.lastmodified)
                {
                    try
                    {
                        _filesRepository.Edit(databaseFile, AWSfile);
                        _filesRepository.Save();
                    }
                    catch (Exception)
                    {

                        throw new Exception("Falha ao atualizar arquivo");
                    }

                }
                else
                {
                    Console.WriteLine("Arquivo {0} não pode ser atualizado pois o presente na base está mais atualizado ", databaseFile.filename);
                }
            }
        }

        public void DeleteMessage(Message obj, string queueUrl, AmazonSQSClient sqsClient)
        {
            var deleteRequest = new DeleteMessageRequest()
            {
                QueueUrl = queueUrl,
                ReceiptHandle = obj.ReceiptHandle
            };
            sqsClient.DeleteMessageAsync(deleteRequest);

            Console.WriteLine("Mensagem Deletada");
        }
    }
}
