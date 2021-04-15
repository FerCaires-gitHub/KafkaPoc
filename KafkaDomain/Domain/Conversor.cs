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
