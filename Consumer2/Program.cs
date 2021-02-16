using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer2
{
    class Program
    {
        static string TOPICO2 = "FernandoTeste";
        static void Main(string[] args)
        {
            var consumer2 = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                GroupId = "FooGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

            try
            {
                using (var myConsumer = new ConsumerBuilder<Null, string>(consumer2)
                    .Build())
                {
                    myConsumer.Subscribe(TOPICO2);
                    while (true)
                    {
                        try
                        {
                            var cr = myConsumer.Consume(cts.Token);
                            var json = JsonConvert.SerializeObject(cr.Message.Value);
                            Console.WriteLine($"Consumer2:{json}");
                        }
                        catch (OperationCanceledException)
                        {

                            myConsumer.Close();
                            Console.WriteLine("Cancelado....");
                        }
                    }
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Erro generico");
            }
        }
    }
}
