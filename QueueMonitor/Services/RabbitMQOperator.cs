using System;
using RabbitMQ.Client;

namespace QueueMonitor.Services
{
    public class RabbitMQOperator : IQueuePlatform
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _model;
        private string _queueName;

        public RabbitMQOperator()
        {
            _factory = new ConnectionFactory();
        }

        public async Task<int> GetMessageCount()
        {
            int messageCount = string.IsNullOrEmpty(_queueName) ? 0
                :(int)_model.MessageCount(_queueName);

            return messageCount;
        }

        public void Initialize(string queueName, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException(nameof(connectionString));

            if (string.IsNullOrEmpty(queueName))
                throw new InvalidOperationException(nameof(queueName));


            _factory.Uri = new Uri(connectionString);
            _queueName = queueName.Trim();
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            
        }
    }
}

