using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Poligonos
{
    public class Dimension
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }
        public void setPosition(int x,int y)
        {
            Position = new Vector2(x, y);
        }
        public void setRotation(int x)
        {
            Rotation = x;
        }
        public void setScale(float x, float y)
        {
            Scale = new Vector2(x, y);
        }
    }
}
