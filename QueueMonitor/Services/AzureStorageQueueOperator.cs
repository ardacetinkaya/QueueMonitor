using System;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace QueueMonitor.Services
{
    public class AzureStorageQueueOperator: IQueuePlatform
    {
        private QueueClient _queueClient;
        private bool _isInitialized = false;

        public async Task<int> GetMessageCount()
        {

            int messageCount = 0;
            if (_isInitialized && _queueClient.Exists())
            {
                QueueProperties properties = await _queueClient.GetPropertiesAsync();
                messageCount=properties.Metadata.ContainsKey("key") ? Convert.ToInt32(properties.Metadata["key"]) : 0;
            }

            return messageCount;
        }

        public void Initialize(string queueName, string connectionString)
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _isInitialized = true;
        }
    }
}

