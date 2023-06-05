using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poligonos
{
    public class Linea
    {
        public int inicio { get; set; }
        public int final { get; set; }  
        public Linea() {
            inicio = 0;
            final = 0;
        }
    }
}
