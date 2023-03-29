using Application.Interfaces;
using Azure.Messaging.ServiceBus;
using Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Application
{
    public class ServiceQueueBusSofkerStatistics : IServiceQueueBus
    {
        private ServiceBusClient _serviceQueueBus;
        public String storageAccountConnection;
        private readonly IConfiguration _configuration;

        public ServiceQueueBusSofkerStatistics(IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceQueueBus = new ServiceBusClient(_configuration.GetSection("ConnectionStorageAccount").Value);
            
        }

        public async Task QueueMessagesAsync(SofkerStadistic sofkerStadistic)
        {
            ServiceBusSender sender = _serviceQueueBus.CreateSender("sofkersstadisticsqueue");
            ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(sofkerStadistic));
            await sender.SendMessageAsync(message);
        }
    }
}
