using Confluent.Kafka;
using KafkaDomain.Domain;
using KafkaDomain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaProducer
{
    public class ProducerBase
    {
        private readonly ProducerConfig config;
        private readonly IDictionary<Type, string> Topicos;
        public ProducerBase()
        {
            config = new ProducerConfig
            {
                BootstrapServers = "omnibus-01.srvs.cloudkafka.com:9094,omnibus-02.srvs.cloudkafka.com:9094,omnibus-03.srvs.cloudkafka.com:9094"
                ,
                SaslUsername = "ynm8ml8b",
                SaslPassword = "Dcd35T6SmpSyh_FfC2r-rrLQUUWAcUl0"
                ,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256
            };

            Topicos = new Dictionary<Type, string>()
            {
                {typeof(User),"ynm8ml8b-TesteKafka" },
                {typeof(Produto),"ynm8ml8b-TesteKafka_2" }
            };
        }

        public void SendMessage<T>(T message) where T : class
        {
            using (var p = new ProducerBuilder<Null, T>(config).
                SetValueSerializer(new CustomSerializer<T>()).
                Build())
            {
                try
                {

                    var header = new Headers() { };
                    header.Add("TYPE", Encoding.UTF8.GetBytes(typeof(T).Name));

                    var dr = p.ProduceAsync(Topicos[typeof(T)], new Message<Null, T> { Headers = header, Value = message }).GetAwaiter().GetResult();
                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");

                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
