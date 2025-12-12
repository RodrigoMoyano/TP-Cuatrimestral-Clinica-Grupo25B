using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class EditarAdmin : PaginaAdmin
    {
        private int idAdmin;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!int.TryParse(Request.QueryString["id"], out idAdmin))
            {
                lblMensaje.Text = "ID invalido";
                return;
            }
            if (!IsPostBack)
            {
                CargarAdmin();
            }
        }

        private void CargarAdmin()
        {
            UsuarioNegocio negocio = new UsuarioNegocio();
            Usuario admin = negocio.ObtenerPorId(idAdmin);

            if (admin == null)
            {
                lblMensaje.Text = "Administrador no encontrado";
                return;
            }

            txtNombre.Text = admin.NombreUsuario;

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionAdmin.aspx");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                lblMensaje.Text = "El nombre de usuario no puede estar vacío.";
                return;
            }

            UsuarioNegocio negocio = new UsuarioNegocio();

            Usuario admin = new Usuario
            {
                Id = idAdmin,
                NombreUsuario = txtNombre.Text.Trim(),
                Clave = string.IsNullOrWhiteSpace(txtClave.Text)
                    ? null
                    : txtClave.Text.Trim()
            };

            negocio.EditarAdmin(admin);
            Response.Redirect("GestionAdmin.aspx");
        }
    }
}