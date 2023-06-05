using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Poligonos
{
    public partial class Form1 : Form
    {
        Dimension dimensionOriginal= new Dimension();
        private int pixelSize { get; set; }
        int W, H, w,h;
        Bitmap bitmap;
        string filePath;
        Graphics g;
        Pen lapiz = new Pen (Color.Black);
        Pen lapizAzul = new Pen(Color.Blue);
        Pen lapizGrisClaro= new Pen (Color.LightGray);
        Pen lapizGrisOscuro=new Pen (Color.DarkGray);

        private void Lienzo_Click(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            pixelSize = 10;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            limpiar();
        }
        void limpiar()
        {
            W = Lienzo.Width;
            H = Lienzo.Height;
            w = W / 2;
            h = H / 2;
            bitmap = new Bitmap(W, H);
            g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            grilla();
            Lienzo.Image = bitmap;
        }
        void refrescar()
        {
            Lienzo.Image = bitmap;
        }
        private void grilla()
        {
            g.Clear(Color.White);
            for (int i = 0; i < h; i+=pixelSize)
            {
                if (i % (5 * pixelSize) == 0)
                {
                    g.DrawLine(lapizGrisOscuro, w - i - pixelSize, 0, w - i - pixelSize, H);
                    g.DrawLine(lapizGrisOscuro, w + i + pixelSize, 0, w + i + pixelSize, H);
                }
                else
                {
                    g.DrawLine(lapizGrisClaro, w - i - pixelSize, 0, w - i - pixelSize, H);
                    g.DrawLine(lapizGrisClaro, w + i + pixelSize, 0, w + i + pixelSize, H);
                }
               
            }
            for (int i = 0; i < w; i += pixelSize)
            {
                if (i % (5 * pixelSize) == 0) {
                    g.DrawLine(lapizGrisOscuro, 0, h - i - pixelSize, W, h - i - pixelSize);
                    g.DrawLine(lapizGrisOscuro, 0, h + i + pixelSize, W, h + i + pixelSize);

                }
                else
                {
                    g.DrawLine(lapizGrisClaro, 0, h - i - pixelSize, W, h - i - pixelSize);
                    g.DrawLine(lapizGrisClaro, 0, h + i + pixelSize, W, h + i + pixelSize);
                }
                   
            }
            
            g.DrawLine(lapiz, w, 0, w, H);
            g.DrawLine(lapiz, 0, h, W, h);

        }
   
        public Poligono leerDatos(string file)
        {
            Poligono pol = new Poligono();
           var text= File.ReadAllText(file);
            pol = JsonSerializer.Deserialize<Poligono>(text);
            return pol;
         
        }
        public void dibujarPoligono(Poligono poli)
        {
            dibujarLineasPoligono(poli);
            pintarAllPuntos(poli);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Seleccionar archivo"; // Título del cuadro de diálogo
            openFileDialog1.Filter = "Archivos JSON (*.json)|*.json";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Obtén la ruta completa del archivo seleccionado
                 filePath = openFileDialog1.FileName;
                this.srcLabel.Text = filePath;
                
                Poligono poligono = leerDatos(filePath);
                limpiar();
                dibujarPoligono(poligono);
                refrescar();
                // Aquí puedes utilizar el archivo seleccionado, por ejemplo, para cargar su contenido en una ventana de tu aplicación.
            }
        }
        void calcular()
        {
            if (!isEmpaty())
            {
                recuperarInfo();
                Poligono poligono = leerDatos(filePath);
                poligono= Trasformar(poligono, dimensionOriginal);
                Console.WriteLine("dibujar");
                limpiar();
                dibujarPoligono(poligono);
                refrescar();
            }
        }
    Poligono Trasformar(Poligono poligono,Dimension dimension)
        {
            poligono=TrasformarPosition(poligono, dimension.Position);
            poligono = TrasformarRotation(poligono,dimension.Rotation);
            poligono = TrasformarScale(poligono,dimension.Scale);
            return poligono;
        }
        Poligono TrasformarPosition(Poligono poligono,Vector2 position)
        {
            foreach (var item in poligono.puntos)
            {
                item.x+=(int)position.X;
                item.y += (int)position.Y;
            }
            return poligono;
        }
        Poligono TrasformarRotation(Poligono poligono, float x)
        {
            double radianes =x * (Math.PI / 180);
            double coseno = Math.Cos(radianes);
            double seno = Math.Sin(radianes);
            foreach (var item in poligono.puntos)
            {
                int xorigial=item.x;
                int yorigial=item.y;
                item.x= (int)Math.Round((xorigial *coseno-yorigial*seno), MidpointRounding.ToEven);
                item.y= (int)Math.Round((xorigial *seno+yorigial*coseno), MidpointRounding.ToEven);
            }
            return poligono;
        }
        Poligono TrasformarScale(Poligono poligono, Vector2 scale)
        {
            foreach (var item in poligono.puntos)
            {
                item.x = (int)(item.x*scale.X);
                item.y = (int)(item.y*scale.Y);
            }
            return poligono;
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true; // Rechaza la tecla presionada.
            }

            // Solo se permite un signo menos al principio del texto
            if (e.KeyChar == '-' && (sender as System.Windows.Forms.TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
            }

            // Solo se permite un punto decimal en el texto
            if (e.KeyChar == '.' && (sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }



        public void pintarAllPuntos(Poligono poli)
        {
            foreach (var item in poli.puntos)
            {
                Vertice(item.x, item.y, item.indice);
            }
        }
        private void Vertice(int x, int y, string nombre)
        {
            x = w + x * pixelSize - pixelSize / 2;
            y = h - y * pixelSize - pixelSize / 2;
            SolidBrush brocha = new SolidBrush(Color.Red);
            SolidBrush brocha2 = new SolidBrush(Color.Black);
            Font fuente = new Font("Arial", (float)(pixelSize * .75));
            SizeF textSize = g.MeasureString(nombre, fuente);
            float textX = x + (pixelSize - textSize.Width) / 2;
            float textY = y + (pixelSize - textSize.Height) / 2;

            g.FillEllipse(brocha, x, y, pixelSize, pixelSize);
            g.DrawString(nombre + "", fuente, brocha2, x, y);
        }
        public void dibujarLineasPoligono(Poligono poli)
        {
            foreach (var item in poli.linea)
            {
                dibujarLineas(new Point(poli.puntos[item.inicio].x, poli.puntos[item.inicio].y), new Point(poli.puntos[item.final].x, poli.puntos[item.final].y));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            calcular();
        }

        public void dibujarLineas(Point inicio,Point final)
        {
            inicio.X = w+inicio.X*pixelSize;
            final.X = w + final.X * pixelSize;

            inicio.Y = h - inicio.Y * pixelSize;
            final.Y = h - final.Y * pixelSize;
            g.DrawLine(lapizAzul, inicio, final);
        }
       void recuperarInfo()
        {
            dimensionOriginal.setPosition(Int32.Parse(this.PositionTxtBox_X.Text), Int32.Parse(this.PositionTxtBox_Y.Text));
            dimensionOriginal.setRotation(Int32.Parse(this.RotationTxtBox_X.Text));
            dimensionOriginal.setScale(float.Parse(this.ScaleTxtBox_X.Text), float.Parse(this.ScaleTxtBox_Y.Text));
        }
        bool isEmpaty()
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(this.PositionTxtBox_X.Text))
            {
                this.PositionTxtBox_X.Text = "0";
                i++;
            }
            if (string.IsNullOrWhiteSpace(this.PositionTxtBox_Y.Text))
            {
                this.PositionTxtBox_Y.Text = "0";
                i++;
            }
            if (string.IsNullOrWhiteSpace(this.RotationTxtBox_X.Text))
            {
                this.RotationTxtBox_X.Text = "0";
                i++;
            }
    
            if (string.IsNullOrWhiteSpace(this.ScaleTxtBox_X.Text))
            {
                this.ScaleTxtBox_X.Text = "1";
                i++;
            }
            if (string.IsNullOrWhiteSpace(this.ScaleTxtBox_Y.Text))
            {
                this.ScaleTxtBox_Y.Text = "1";
                i++;
            }
            if (i>0)
            {
                return true;
            }
            else { return false; }
        }
    }
 
 
}
