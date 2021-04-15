using KafkaDomain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaDomain.Domain
{
    public class Produto : IModelBase
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
    }
}
