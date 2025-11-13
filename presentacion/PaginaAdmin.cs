using Dominio;
using System;
using System.Web.UI;

namespace presentacion
{
    //Hereda de System.Web.UI.Page
    public class PaginaAdmin : System.Web.UI.Page
    {
        //Usamos Oninit, se ejecuta antes del page_Load de la página hija
        protected override void OnInit(EventArgs e)
        {

            // Chequea si el usuario NO es Admin
            if (!Seguridad.esAdmin(Session["usuario"]))
            {
                // Si no lo es, lo saca
                Session.Add("error", "No tienes permisos para esta página.");
                Response.Redirect("Error.aspx", false);
                return;
            }

            //si es Admin, continúa cargando la página normalmente
            base.OnInit(e);
        }
    }
}