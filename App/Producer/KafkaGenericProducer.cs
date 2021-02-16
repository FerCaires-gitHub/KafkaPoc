using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Producer
{
    public class KafkaGenericProducer<T> : IKafkaGenericProducer<T>
    {
        private ProducerConfig _config;
        public KafkaGenericProducer(string bootstrapServer = "")
        {
            _config = new ProducerConfig { BootstrapServers = bootstrapServer };
        }

        public async Task Publish(string topic, T key, string message)
        {
            using (var producer = new ProducerBuilder<T, string>(_config).Build())
            {
                var kafkaMessage = new Message<T, string> { Key = key, Value = message, Timestamp = Timestamp.Default };
                await producer.ProduceAsync(topic, kafkaMessage);
            }
        }

        public Task Publish(IEnumerable<string> topic, T key, string message)
        {
            throw new NotImplementedException();
        }

        public Task Publish(IEnumerable<string> topic, T key, IEnumerable<string> message)
        {
            throw new NotImplementedException();
        }

        public Task Publish(string topic, T key, IEnumerable<string> message)
        {
            throw new NotImplementedException();
        }

        public async Task Publish(string topic, string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                try
                {
                    var kafkaMessage = new Message<Null, string> { Value = message, Timestamp = Timestamp.Default };
                    await producer.ProduceAsync(topic, kafkaMessage);
                }
                catch (KafkaException kex)
                {
                    Console.WriteLine($"Erro ao publicar mensagem no tópico:{topic}. Descrição:{kex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro genérico. Descrição:{ex.Message}");
                }

            }
        }

        public async Task Publish(string topic, IEnumerable<string> messages)
        {
            foreach (var message in messages)
            {
                await Publish(topic, message);
            }
        }

        public async Task Publish(IEnumerable<string> topics, string message)
        {
            foreach (var topic in topics)
            {
                await Publish(topic, message);
            }
        }

        public async Task Publish(IEnumerable<string> topics, IEnumerable<string> messages)
        {
            foreach (var topic in topics)
            {
                await Publish(topic, messages);
            }
        }
    }
}
