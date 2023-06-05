using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poligonos
{
    public class Poligono
    {  public List<Puntos> puntos { get; set; }
        public List<Linea> linea { get; set; }
        public Poligono() {
            puntos = new List<Puntos>();
            linea = new List<Linea>();
        }   
    }
}
