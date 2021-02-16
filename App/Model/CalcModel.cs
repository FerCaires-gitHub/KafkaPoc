using App.Delegates;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Delegates.CalculosDelegate;

namespace App.Model
{
    public class CalcModel
    {
        [JsonProperty("Numero",NullValueHandling =NullValueHandling.Ignore)]
        public int Numero { get; set; }
        [JsonProperty("Dobro", NullValueHandling = NullValueHandling.Ignore)]
        public int Dobro { get; set; }
        [JsonProperty("Quadrado", NullValueHandling = NullValueHandling.Ignore)]
        public int Quadrado { get; set; }
        [JsonProperty("Cubo", NullValueHandling = NullValueHandling.Ignore)]
        public int Cubo { get; set; }

        private CalcDelegate Calculos;

        public CalcModel(int number)
        {
            
            Numero = number;
            Dobro = default(int);
            Quadrado = default(int);
            Cubo = default(int);
            Calculos = new CalcDelegate(Double);
            Calculos += Square;
            Calculos += Cube;
            Calculos(this);
            
        }

        public void Double(CalcModel valor) => Dobro = valor.Numero * 2;
        public void Square(CalcModel valor) => Quadrado = Convert.ToInt32(Math.Pow(valor.Numero, 2));
        public void Cube(CalcModel valor) => Cubo = Convert.ToInt32(Math.Pow(valor.Numero, 3));
    }
}
