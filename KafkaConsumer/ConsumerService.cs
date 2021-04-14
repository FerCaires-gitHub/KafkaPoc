using Confluent.Kafka;
using KafkaDomain.Annotation;
using KafkaDomain.Domain;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading;

namespace KafkaConsumer
{

    public class ConsumerService<T> where T : class
    {
        private string Topico = string.Empty;

        public ConsumerService()
        {
            GetTopico();
        }

        private void GetTopico()
        {
            var attribute = (KafkaListenerAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(KafkaListenerAttribute));
            if (attribute != null)
                Topico = attribute.Topico;
        }

        public void Consume()
        {
            var config = new ConsumerConfig()
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "omnibus-01.srvs.cloudkafka.com:9094,omnibus-02.srvs.cloudkafka.com:9094,omnibus-03.srvs.cloudkafka.com:9094",
                SaslUsername = "ynm8ml8b",
                SaslPassword = "Dcd35T6SmpSyh_FfC2r-rrLQUUWAcUl0",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256,
                AutoOffsetReset = AutoOffsetReset.Latest,
                //EnablePartitionEof = true,
                EnableAutoCommit = true
            };

            using (var c = new ConsumerBuilder<Null, T>(config)
                .SetValueDeserializer(new CustomDeserializer<T>())
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build())
            {
                c.Subscribe(Topico);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        var cr = new ConsumeResult<Null, T>();
                        try
                        {
                            cr = c.Consume(cts.Token);
                            if (cr.Message != null)
                                Console.WriteLine($"Consumed message id '{JsonConvert.SerializeObject(cr.Message.Value)}' at: '{cr.TopicPartitionOffset}'.");
                            else
                                cr.Message = new Message<Null, T>();
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
            }
        }
    }
}
