using KafkaDomain.Annotation;
using KafkaDomain.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaConsumer
{
    [KafkaListener("ynm8ml8b-HelloWorld")]
    public class UserConsumerService:ConsumerService<User>
    {

    }
}
