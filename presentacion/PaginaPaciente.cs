using Dominio;
using System;
using System.Web.UI;

namespace presentacion
{
    public class PaginaPaciente : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            if (!Seguridad.esPaciente(Session["usuario"]) && !Seguridad.esAdmin(Session["usuario"]))
            {
                Session.Add("error", "No tienes permisos para esta página.");
                Response.Redirect("Error.aspx", false);
                return;
            }

            base.OnInit(e);
        }
    }
}