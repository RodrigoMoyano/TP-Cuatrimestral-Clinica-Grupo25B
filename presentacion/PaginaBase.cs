using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace presentacion
{
    public class PaginaBase : Page
    {
        protected override void OnInit(EventArgs e)
        {
            if (!Seguridad.sessionActiva(Session["usuario"]))
            {
                Response.Redirect("Login.aspx", false);
                return;
            }
            base.OnInit(e);
        }
    }
}