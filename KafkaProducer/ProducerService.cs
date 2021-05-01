using Confluent.Kafka;
using KafkaDomain.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaProducer
{
    public class ProducerService
    {

        public IList<string> Topicos = new List<string>() { "ynm8ml8b-TesteKafka_2", "ynm8ml8b-TesteKafka" };
        
        public void SendMessage()
        {
            var random = new Random();
            var config = new ProducerConfig { 
                BootstrapServers = "omnibus-01.srvs.cloudkafka.com:9094,omnibus-02.srvs.cloudkafka.com:9094,omnibus-03.srvs.cloudkafka.com:9094"
                ,SaslUsername = "ynm8ml8b", 
                SaslPassword = "Dcd35T6SmpSyh_FfC2r-rrLQUUWAcUl0" 
                ,SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256
            };

            var user = new User { Id = 100, Email = "teste@teste.com", Nome = "Teste" };
            var produto = new Produto { Id = 1, Nome = "Televisao", Valor = 10 };

            using (var p = new ProducerBuilder<Null, User>(config).
                SetValueSerializer(new CustomSerializer<User>()).
                Build())
            {
                try
                {
                    
                    var header = new Headers() { };
                    header.Add("TYPE", Encoding.UTF8.GetBytes(typeof(User).Name));
                    for (int i = 0; i < 10; i++)
                    {
                        var dr = p.ProduceAsync(Topicos[random.Next(2)], new Message<Null, User> { Headers = header, Value = user }).GetAwaiter().GetResult();
                        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                        user.Id++;
                    }
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }

            using (var p = new ProducerBuilder<Null, Produto>(config).
                SetValueSerializer(new CustomSerializer<Produto>()).
                Build())
            {
                try
                {
                    var header = new Headers() { };
                    header.Add("TYPE", Encoding.UTF8.GetBytes(typeof(Produto).Name));
                    for (int i = 0; i < 10; i++)
                    {
                        var dr = p.ProduceAsync(Topicos[random.Next(2)], new Message<Null, Produto> { Headers = header, Value = produto }).GetAwaiter().GetResult();
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
