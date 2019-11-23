using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Captcha
{
    /// <summary>
    /// Summary description for GerarCaptcha
    /// </summary>
    public class GerarCaptcha : IHttpHandler, IRequiresSessionState
    {
        private HttpContext _context;
        private WebSoccer.Services.Captcha _img;

        public void ProcessRequest(HttpContext context)
        {
            _context = context;
            CriarImagemSafe();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        private bool possoSetarImagem()
        {
            return _context.Session["timeKey"] == null;
        }

        private void SetSessionImgKey(string valor)
        {
            _context.Session["imgKey"] = _img.GeraImage(_context, valor);
        }

        private string GetSessionImgKey()
        {
            return _context.Session["imgKey"] != null ? _context.Session["imgKey"].ToString() : string.Empty;
        }

        private void SetTimeKey()
        {
            _context.Session["timeKey"] = _context.Timestamp;
        }

        private void RemoveTimeKey()
        {
            _context.Session.Remove("timeKey");
        }

        private void CriarImagemSafe()
        {
            _img = new WebSoccer.Services.Captcha();
            _img.BackColor = System.Drawing.Brushes.Chocolate;
            _img.Color = System.Drawing.Brushes.White;
            _img.PointColor = System.Drawing.Color.White;

            //Se é pra colocar números no captcha ou não
            _img.Numeros = false;

            _img.TotalCaracteres = 3;
            _img.Height = 61;
            _img.Width = 146;

            ExibirImagem();
        }

        private void ExibirImagem()
        {
            if (possoSetarImagem())
            {
                SetSessionImgKey(string.Empty);
                SetTimeKey();
            }
            else
            {
                RemoveTimeKey();
                SetSessionImgKey(GetSessionImgKey());
            }
        }
    }
}