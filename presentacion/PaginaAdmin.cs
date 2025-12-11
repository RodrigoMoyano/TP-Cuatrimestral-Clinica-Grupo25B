using Dominio;
using System;
using System.Web.UI;

namespace presentacion
{
    //Hereda de paginaBase
    public class PaginaAdmin : PaginaBase
    {
        //Usamos Oninit, se ejecuta antes del page_Load de la página hija
        protected override void OnInit(EventArgs e)
        {
            //si es Admin, continúa cargando la página normalmente
            base.OnInit(e);

            //Chequea si el usuario NO es Admin
            if (!Seguridad.esAdmin(Session["usuario"]))
            {
                //Si no lo es lo saca
                Session.Add("error", "No tienes permisos para esta página.");
                Response.Redirect("Menu.aspx", false);
                return;
            }

        }
    }
}