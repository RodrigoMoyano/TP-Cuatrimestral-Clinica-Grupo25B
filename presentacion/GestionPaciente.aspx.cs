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
    public partial class GestionPaciente : System.Web.UI.Page
    {
        private readonly PacienteNegocio negocio = new PacienteNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarGrilla();
        }
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPacientes();
        }

        private void CargarPacientes()
        {
            PacienteNegocio negocio = new PacienteNegocio();
            string filtro = ddlFiltro.SelectedValue;

            var lista = negocio.ListarFiltrado(filtro);

            gvPacientes.DataSource = lista;
            gvPacientes.DataBind();
        }

        private void CargarGrilla()
        {
            try
            {
                gvPacientes.DataSource = negocio.ObtenerTodos();
                gvPacientes.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar pacientes: " + ex.Message;
            }
        }

        protected void btnIrRegistro_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registro.aspx?admin=1");
        }

        protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int idPaciente = Convert.ToInt32(gvPacientes.DataKeys[index].Value);

                Response.Redirect("EditarPaciente.aspx?id=" + idPaciente);
            }
        }
    }
}