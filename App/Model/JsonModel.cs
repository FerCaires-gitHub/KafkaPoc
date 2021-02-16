using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    public class JsonModel
    {
        [JsonProperty("Chave",NullValueHandling =NullValueHandling.Ignore)]
        public int Chave { get; set; }
        [JsonProperty("Valor", NullValueHandling = NullValueHandling.Ignore)]
        public string Valor { get; set; }
    }
}
