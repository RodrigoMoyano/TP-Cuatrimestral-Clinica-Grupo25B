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
    public partial class GestionAdmin : PaginaAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarAdmin();
            }
        }
        private void cargarAdmin()
        {
            UsuarioNegocio negocio = new UsuarioNegocio();
            string estado = ddlEstado.SelectedValue;
            
            dgvAdmin.DataSource = negocio.ListarAdmin(estado);
            dgvAdmin.DataBind();
        }

        protected void btnAgregarAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgregarAdmin.aspx");
        }

        protected void dgvAdmin_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            UsuarioNegocio negocio = new UsuarioNegocio();

            if (e.CommandName == "Desactivar")
            {
                negocio.EstadoAdmin(id, false);

                cargarAdmin();
            }
            if (e.CommandName == "Activar")
            {
                negocio.EstadoAdmin(id, true);
                cargarAdmin();
            }

            if (e.CommandName == "EditarAdmin")
            {
                Response.Redirect("EditarAdmin.aspx?id=" + id);
            }
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarAdmin();
        }
    }
}