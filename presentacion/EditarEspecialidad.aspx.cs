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
    public partial class EditarEspecialidad : PaginaAdmin
    {
        private int idEspecialidad;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["Usuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Request.QueryString["Id"] != null)
            {
                idEspecialidad = int.Parse(Request.QueryString["Id"]);
            }
            else
            {
                Response.Redirect("Especialidades.aspx");
            }

            if (!IsPostBack)
            {
                CargarEspecialidad();
            }
        }

        private void CargarEspecialidad()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            Especialidad esp = negocio.BuscarPorId(idEspecialidad);

            if (esp == null)
            {
                Response.Redirect("Especialidades.aspx");
                return;
            }

            txtDescripcion.Text = esp.Descripcion;
            txtImagen.Text = esp.Imagen;
            imgPreview.ImageUrl = esp.Imagen;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Especialidad esp = new Especialidad
            {
                Id = idEspecialidad,
                Descripcion = txtDescripcion.Text,
                Imagen = txtImagen.Text
            };

            EspecialidadNegocio negocio = new EspecialidadNegocio();
            negocio.Modificar(esp);
            divExito.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "redir",
                "setTimeout(function(){ window.location = 'Especialidades.aspx'; }, 2000);", true);
            
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            negocio.Eliminar(idEspecialidad);

            divEliminar.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "redir",
                "setTimeout(function(){ window.location = 'Especialidades.aspx'; }, 2000);", true);
            
        }
    }
}