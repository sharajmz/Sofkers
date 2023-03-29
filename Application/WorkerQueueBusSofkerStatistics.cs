using Application.Interfaces;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Application
{
    public class WorkerQueueBusSofkerStatistics : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private ServiceBusClient _serviceQueueBus;
        private List<string> _queueMessages = new List<string>();
        private IAdapterSofkerStatistic _adapterSofkerStatistic;

        public WorkerQueueBusSofkerStatistics(IConfiguration configuration, IAdapterSofkerStatistic adapterSofkerStatistic)
        {
            _configuration = configuration;
            _serviceQueueBus = new ServiceBusClient(_configuration.GetSection("ConnectionStorageAccount").Value);
            _adapterSofkerStatistic = adapterSofkerStatistic;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ReceiveMessageAsync();
                await Task.Delay(4000, stoppingToken);
            }
        }

        private async Task ReceiveMessageAsync()
        {
            try
            {
                ServiceBusProcessor processor = _serviceQueueBus.CreateProcessor(_configuration.GetSection("QueueName").Value);
                processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
                processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
                await processor.StartProcessingAsync();
                Thread.Sleep(3000);
                await processor.StopProcessingAsync();
                if(await _adapterSofkerStatistic.InsertSofkertChangesLog(_queueMessages))
                {
                    _queueMessages.Clear();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($" Exception Throwed {ex.Message}");
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
