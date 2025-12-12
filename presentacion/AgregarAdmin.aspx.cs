using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class AgregarAdmin : PaginaAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate();
                if(!Page.IsValid)
                {
                    return;
                }

                Usuario nuevo = new Usuario();
                nuevo.NombreUsuario = txtUsuario.Text.Trim();
                nuevo.Clave = txtClave.Text.Trim();

                UsuarioNegocio negocio = new UsuarioNegocio();

                if (negocio.NombreUsuarioExiste(nuevo.NombreUsuario, 0))
                {
                    lblError.Text = "Nombre de usuario existente";
                    lblError.Visible = true;
                    return;
                }
                
                negocio.AgregarAdmin(nuevo);

                Response.Redirect("GestionAdmin.aspx");
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionAdmin.aspx");
        }
    }
}