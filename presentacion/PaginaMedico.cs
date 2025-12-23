using Dominio;
using System;
using System.Web.UI;

namespace presentacion
{
    public class PaginaMedico : PaginaBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!Seguridad.esMedico(Session["usuario"]) && !Seguridad.esAdmin(Session["usuario"]))
                Session.Add("error", "No tienes permisos para esta página.");
                Response.Redirect("Menu.aspx", false);
                return;
            }

        }
    }
}