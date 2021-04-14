using Confluent.Kafka;
using KafkaDomain.Domain;
using System;
using System.Text;

namespace KafkaProducer
{
    public class ProducerService
    {
        
        public void SendMessage()
        {
            var config = new ProducerConfig { BootstrapServers = "omnibus-01.srvs.cloudkafka.com:9094,omnibus-02.srvs.cloudkafka.com:9094,omnibus-03.srvs.cloudkafka.com:9094"
                , SaslUsername = "ynm8ml8b", SaslPassword = "Dcd35T6SmpSyh_FfC2r-rrLQUUWAcUl0" 
                ,SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256
            };
            var user = new User { Id = 100, Email = "teste@teste.com", Nome = "Teste" };

            using (var p = new ProducerBuilder<Null, User>(config).
                SetValueSerializer(new CustomSerializer<User>()).
                Build())
            {
                try
                {
                    var header = new Headers() { };
                    header.Add("Topic", Encoding.UTF8.GetBytes("TesteHeader"));
                    for (int i = 0; i < 10; i++)
                    {
                        var dr = p.ProduceAsync("ynm8ml8b-HelloWorld", new Message<Null, User> { Headers = header, Value = user }).GetAwaiter().GetResult();
                        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                        user.Id++;
                    }
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
