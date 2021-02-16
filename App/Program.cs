using App.Model;
using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App
{
    class Program
    {
        static string TOPICO = "FernandoTeste";
        static string TOPICO2 = "FernandoTopic";
        static async Task Main(string[] args)
        {
            var config = new ProducerConfig() { BootstrapServers = "localhost:9092" };

            Console.WriteLine("Escrevendo as mensagens no tópico");
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {

                for (int i = 0; i < 150; i++)
                {
                    var model = new CalcModel(i);
                    var message = new Message<Null, string> { Value = JsonConvert.SerializeObject(model) };
                    await producer.ProduceAsync(TOPICO, message);
                    await producer.ProduceAsync(TOPICO2, message);
                }

            }
            Console.WriteLine("Mensagens escritas com sucesso!");

            var consumerConfig = new ConsumerConfig()
            {

                BootstrapServers = "localhost:9092",
                GroupId = "MyGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };
            try
            {
                using (var consumer = new ConsumerBuilder<Null, string>(consumerConfig)
                    .Build())
                {
                    consumer.Subscribe(TOPICO);
                    while (true)
                    {
                        try
                        {
                            var cr = consumer.Consume(cts.Token);
                            var json = JsonConvert.SerializeObject(cr.Message.Value);
                            Console.WriteLine($"Message:{json}");
                        }
                        catch (OperationCanceledException)
                        {

                            consumer.Close();
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

        internal static string GetStringByNumber(int number)
        {
            var retorno = "";
            if (number % 2 == 0) retorno = "par";
            if (number % 2 != 0) retorno = "impar";
            if (number % 3 == 0) retorno = $"{retorno}-Multiplo de 3";
            if (number % 5 == 0) retorno = $"{retorno}-Multiplo de 5";
            if (number % 7 == 0) retorno = $"{retorno}-Multiplo de 7";
            if (string.IsNullOrEmpty(retorno)) retorno = "Cansei";
            return retorno;


        }

    }
}
