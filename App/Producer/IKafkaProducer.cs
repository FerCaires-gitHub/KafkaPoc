using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Producer
{
    public interface IKafkaProducer
    {
        Task Publish(string topic, string message);
        Task Publish(string topic, IEnumerable<string> messages);
        Task Publish(IEnumerable<string> topics, string message);
        Task Publish(IEnumerable<string> topics, IEnumerable<string> messages);
        
    }
}
