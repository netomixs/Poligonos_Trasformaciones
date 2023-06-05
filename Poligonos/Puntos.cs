using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poligonos
{
    public class Puntos
    {

        public int x { get; set; }
        public int y { get; set; }
        public string indice { get; set; }
        public Puntos()
        {
            this.x = 0;
            this.y = 0;
        }
        public Puntos(string indice,int x,int y)
        {
            this.x = x;
            this.y = y;
            this.indice = indice;
        }
    }
}
