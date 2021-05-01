using Confluent.Kafka;
using KafkaDomain.Enums;
using KafkaDomain.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaDomain.Domain
{
    public class Conversor
    {
        private static IDictionary<string, Tipos> _dictionaryMapper = new Dictionary<string, Tipos>()
        {
            { "USER", Tipos.User},
            { "PRODUTO", Tipos.Produto}
        };

        private static IDictionary<string, dynamic> conversorMapper = new Dictionary<string, dynamic>()
        { 
            {"ynm8ml8b-TesteKafka_2", new CustomDeserializer<User>() },
            {"ynm8ml8b-TesteKafka", new CustomDeserializer<Produto>()}
        };


        public static IDeserializer<IModelBase> GetDeserializer(string topicName)
        {
            IDeserializer<IModelBase> conversor = null;
            var retorno = typeof(IDeserializer<>);
            var constructed = retorno.MakeGenericType(typeof(IModelBase));
            var result = Activator.CreateInstance(constructed);
            if (conversorMapper.ContainsKey(topicName))
            {
                conversor = conversorMapper[topicName];
            }
            return conversor;
        }

        private const string TIPO = "TYPE";

        public static IModelBase Mapper(Headers headers, string message)
        {
            var modelo = GetModelo(headers).ToUpper();
            Tipos tipo;
            _dictionaryMapper.TryGetValue(modelo, out tipo);

            IModelBase retorno = null;
            switch (tipo)
            {
                case Tipos.User:
                    retorno = JsonConvert.DeserializeObject<User>(message);
                    break;
                case Tipos.Produto:
                    retorno = JsonConvert.DeserializeObject<Produto>(message);
                    break;
                default:
                    break;
            }

            return retorno;
        }

        private static string GetModelo(Headers headers)
        {
            var modelo = string.Empty;
            for (int i = 0; i < headers.Count; i++)
            {
                var key = headers[i].Key;
                if (key == TIPO)
                {
                    modelo = Encoding.UTF8.GetString(headers[i].GetValueBytes());
                    break;
                }
            }
            return modelo;
        }
    }
}
