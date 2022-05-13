using Amazon.SQS;
using AWSQueueProject.Model;
using AWSQueueProject.Model.Context;
using AWSQueueProject.Model.Interfaces;
using AWSQueueProject.Model.Repositorys;
using AWSQueueProject.Services;
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
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider =serviceCollection.BuildServiceProvider();

            var eventService = serviceProvider.GetService<IApplicationService>();

            Console.WriteLine("Começando aplicação AWS");
            eventService.startService();

            
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            var connection = Environment.GetEnvironmentVariable("MySqlConnectionString");
            services.AddScoped<IApplicationService ,ApplicationService>()
                .AddScoped<IGenericRepository<File>, SqlRepository<File>>()
                .AddScoped<ISQSQueueService,SQSQueueService>()
                .AddDbContext<SQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version())));
        }
    }
}
