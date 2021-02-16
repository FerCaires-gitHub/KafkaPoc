using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Producer
{
    public interface IKafkaGenericProducer<T> : IKafkaProducer
    {
        Task Publish(string topic, T key, string message);
        Task Publish(IEnumerable<string> topic, T key, string message);
        Task Publish(IEnumerable<string> topic, T key, IEnumerable<string> message);
        Task Publish(string topic, T key, IEnumerable<string> message);
    }
}
