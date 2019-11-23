using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace WebSoccer.Services
{
    public class Captcha
    {
        #region Atributos

        Random rnd = new Random();

        System.Drawing.Brush _color;

        System.Drawing.Brush _backColor;

        int _height;

        int _width;

        int _totalCaracteres;

        bool _numeros;

        System.Drawing.Color _pointColor;

        #endregion

        #region Propriedades

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public System.Drawing.Brush BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        public System.Drawing.Brush Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public int TotalCaracteres
        {
            get { return _totalCaracteres; }
            set { _totalCaracteres = value; }
        }

        public bool Numeros
        {
            get { return _numeros; }
            set { _numeros = value; }
        }

        public System.Drawing.Color PointColor
        {
            get { return _pointColor; }
            set { _pointColor = value; }
        }

        #endregion

        #region Métodos

        protected string ImageString()
        {
            String valor = "";

            for (int i = 0; i < _totalCaracteres; i++)
            {
                if (rnd.Next(2) == 1 && _numeros)
                    valor += (char)rnd.Next(48, 56);
                else
                    valor += (char)rnd.Next(65, 90);
            }

            return valor;
        }

        public string GeraImage(HttpContext pagina, string imgKey)
        {
            string strValida = imgKey;

            if (string.IsNullOrEmpty(strValida))
                strValida = ImageString();

            pagina.Response.ContentType = "image/jpeg";
            pagina.Response.Clear();
            pagina.Response.BufferOutput = true;

            System.Drawing.Bitmap img = new System.Drawing.Bitmap(_width, _height);
            System.Drawing.Graphics dr = System.Drawing.Graphics.FromImage(img);

            dr.FillRectangle(_backColor, new System.Drawing.Rectangle(0, 0, img.Width, img.Height));

            System.Drawing.Font font = new System.Drawing.Font("Verdana", 18, System.Drawing.FontStyle.Bold);
            dr.DrawString(strValida, font, _color, rnd.Next(20), rnd.Next(_height - 24));

            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(0, rnd.Next(_height)), new System.Drawing.Point(_width, rnd.Next(_height)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(0, rnd.Next(_height)), new System.Drawing.Point(_width, rnd.Next(_height)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(0, rnd.Next(_height)), new System.Drawing.Point(_width, rnd.Next(_height)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(0, rnd.Next(_height)), new System.Drawing.Point(_width, rnd.Next(_height)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(0, rnd.Next(_height)), new System.Drawing.Point(_width, rnd.Next(_height)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(rnd.Next(_width), 0), new System.Drawing.Point(_height, rnd.Next(_width)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(rnd.Next(_width), 0), new System.Drawing.Point(_height, rnd.Next(_width)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(rnd.Next(_width), 0), new System.Drawing.Point(_height, rnd.Next(_width)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(rnd.Next(_width), 0), new System.Drawing.Point(_height, rnd.Next(_width)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(rnd.Next(_width), 0), new System.Drawing.Point(_height, rnd.Next(_width)));


            for (int x = 0; x < img.Width; x++)
                for (int y = 0; y < img.Height; y++)
                    if (rnd.Next(6) == 1)
                        img.SetPixel(x, y, _pointColor);

            font.Dispose();
            dr.Dispose();

            img.Save(pagina.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            img.Dispose();

            pagina.Response.Write(strValida);
            return strValida;
        }

        public string GeraImage(Page pagina, string imgKey)
        {
            string strValida = imgKey;

            if (string.IsNullOrEmpty(strValida))
                strValida = ImageString();

            pagina.Response.ContentType = "image/jpeg";
            pagina.Response.Clear();
            pagina.Response.BufferOutput = true;

            System.Drawing.Bitmap img = new System.Drawing.Bitmap(_width, _height);
            System.Drawing.Graphics dr = System.Drawing.Graphics.FromImage(img);

            dr.FillRectangle(_backColor, new System.Drawing.Rectangle(0, 0, img.Width, img.Height));

            System.Drawing.Font font = new System.Drawing.Font("Verdana", 18, System.Drawing.FontStyle.Bold);
            dr.DrawString(strValida, font, _color, rnd.Next(20), rnd.Next(_height - 24));

            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(0, rnd.Next(_height)), new System.Drawing.Point(_width, rnd.Next(_height)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(0, rnd.Next(_height)), new System.Drawing.Point(_width, rnd.Next(_height)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(0, rnd.Next(_height)), new System.Drawing.Point(_width, rnd.Next(_height)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(0, rnd.Next(_height)), new System.Drawing.Point(_width, rnd.Next(_height)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(0, rnd.Next(_height)), new System.Drawing.Point(_width, rnd.Next(_height)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(rnd.Next(_width), 0), new System.Drawing.Point(_height, rnd.Next(_width)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(rnd.Next(_width), 0), new System.Drawing.Point(_height, rnd.Next(_width)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(rnd.Next(_width), 0), new System.Drawing.Point(_height, rnd.Next(_width)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(rnd.Next(_width), 0), new System.Drawing.Point(_height, rnd.Next(_width)));
            dr.DrawLine(new System.Drawing.Pen(_color), new System.Drawing.Point(rnd.Next(_width), 0), new System.Drawing.Point(_height, rnd.Next(_width)));


            for (int x = 0; x < img.Width; x++)
                for (int y = 0; y < img.Height; y++)
                    if (rnd.Next(6) == 1)
                        img.SetPixel(x, y, _pointColor);

            font.Dispose();
            dr.Dispose();

            img.Save(pagina.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            img.Dispose();

            return strValida;
        }



        #endregion
    }
}