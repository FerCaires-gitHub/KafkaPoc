using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Delegates
{
    public class ProducerDelegate
    {
        public delegate void PublishMessage<T>(string topic, T message);
    }
}
