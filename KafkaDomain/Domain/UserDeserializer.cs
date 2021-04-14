using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaDomain.Domain
{
    public class UserDeserializer : IDeserializer<User>
    {
        public User Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(data));
        }
    }
}
