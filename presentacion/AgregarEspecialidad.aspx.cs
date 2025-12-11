using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class AgregarEspecialidad : PaginaAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            Usuario usuario = Session["Usuario"] as Usuario;
            if (usuario == null || usuario.Rol == null || usuario.Rol.Id != 1)
            {
                
                Response.Redirect("Menu.aspx", false);
                return;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            
            lblMensaje.Text = "";
            try
            {
                
                string desc = txtDescripcion.Text?.Trim();
                if (string.IsNullOrEmpty(desc))
                {
                    lblMensaje.CssClass = "text-danger";
                    lblMensaje.Text = "Debe ingresar una descripción.";
                    return;
                }

                string imagen = txtImagen.Text?.Trim();
                if (string.IsNullOrEmpty(imagen))
                {
                    lblMensaje.CssClass = "text-danger";
                    lblMensaje.Text = "Debe ingresar una URL de imagen.";
                    return;
                }

                Especialidad nueva = new Especialidad
                {
                    Descripcion = desc,
                    Imagen = imagen
                };

                EspecialidadNegocio negocio = new EspecialidadNegocio();
                negocio.Agregar(nueva);

                lblMensaje.CssClass = "text-success";
                lblMensaje.Text = "Especialidad agregada correctamente.";

                divExito.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "redir",
                    "setTimeout(function(){ window.location = 'Especialidades.aspx'; }, 2000);", true);

                
            }
            catch (Exception ex)
            {
                lblMensaje.CssClass = "text-danger";
                lblMensaje.Text = "Ocurrió un error: " + ex.Message;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Especialidades.aspx", false);
        }
    }
}