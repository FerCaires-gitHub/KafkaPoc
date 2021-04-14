using System;

namespace KafkaProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new ProducerService();
            service.SendMessage();
        }

    }

}
