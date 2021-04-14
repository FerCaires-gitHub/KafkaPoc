using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaDomain.Annotation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class KafkaListenerAttribute:Attribute
    {
        public string Topico { get; set; }
        public string GroupId { get; set; }

        public KafkaListenerAttribute(string topico)
        {
            this.Topico = topico;
        }
    }
}
