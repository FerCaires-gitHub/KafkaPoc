using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaDomain.Interface
{
    public interface IModelBase
    {
         int Id { get; set; }
         string Nome { get; set; }
    }
}
