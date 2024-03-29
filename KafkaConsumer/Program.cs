﻿using Confluent.Kafka;
using KafkaDomain.Domain;
using KafkaDomain.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KafkaConsumer
{
    class Program
    {
        private static IDictionary<string, CustomDeserializer<IModelBase>> dict = new Dictionary<string, CustomDeserializer<IModelBase>>();
        static void Main(string[] args)
        {
            //var consumer = new UserConsumerService();
            //consumer.Consume();
            Consume();
        }
        private static void Consume()
        {
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "omnibus-01.srvs.cloudkafka.com:9094,omnibus-02.srvs.cloudkafka.com:9094,omnibus-03.srvs.cloudkafka.com:9094",
                SaslUsername = "ynm8ml8b",
                SaslPassword = "Dcd35T6SmpSyh_FfC2r-rrLQUUWAcUl0",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            var counter = 0;
            using (var c = new ConsumerBuilder<Null, string>(conf)
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build())
            {
                var topics = new List<string>() { "ynm8ml8b-BetFairData" };

                c.Subscribe(topics);

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
                        var cr = new ConsumeResult<Null, string>();
                        try
                        {
                            cr = c.Consume(cts.Token);
                            counter++;
                            Console.WriteLine($"Counter:{counter}");
                            if (cr.Message != null)
                            {
                                Console.WriteLine(cr.Topic);
                                Console.WriteLine(JsonConvert.SerializeObject(cr.Message.Value));
                                //var deserializer = Conversor.GetDeserializer(cr.Topic);
                                //var result = deserializer.Deserialize(Encoding.UTF8.GetBytes(cr.Message.Value),false,new SerializationContext());
                                //var obj = Conversor.Mapper(cr.Message.Headers, cr.Message.Value);
                                //Console.WriteLine($"Consumed message id '{JsonConvert.SerializeObject(obj)}' at: '{cr.TopicPartitionOffset}'.");
                            }
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
