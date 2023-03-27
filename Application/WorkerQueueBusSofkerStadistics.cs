using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Application
{
    public class WorkerQueueBusSofkerStadistics : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private ServiceBusClient _serviceQueueBus;
        private List<string> _queueMessages = new List<string>();

        public WorkerQueueBusSofkerStadistics(IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceQueueBus = new ServiceBusClient(_configuration.GetSection("ConnectionStorageAccount").Value);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ReceiveMessageAsync();
                await Task.Delay(100, stoppingToken);
            }
        }

        private async Task<List<string>> ReceiveMessageAsync()
        {
            try
            {
                ServiceBusProcessor processor = _serviceQueueBus.CreateProcessor(_configuration.GetSection("QueueName").Value);
                processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
                processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
                await processor.StartProcessingAsync();
                Thread.Sleep(3000);
                await processor.StopProcessingAsync();
                return _queueMessages;

            }
            catch (Exception ex)
            {
                Console.WriteLine($" Exception Throwed {ex.Message}");
                return _queueMessages = new List<string>();
            }

        }

        private async Task Processor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            string content = arg.Message.Body.ToString();
            _queueMessages.Add(content);
            await arg.CompleteMessageAsync(arg.Message);
        }

        private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            return Task.CompletedTask;
        }
    }
}
