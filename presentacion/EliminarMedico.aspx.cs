using System;
using System.Linq;
using System.Web.UI;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class EliminarMedico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["IdMedicoEliminar"] == null)
                {
                    Response.Redirect("GestionMedicos.aspx");
                    return;
                }

                int idMedico = (int)Session["IdMedicoEliminar"];

                MedicoNegocio negocio = new MedicoNegocio();
                // ✅ Forzamos el tipo del dominio para evitar el conflicto de namespace
                Dominio.Medico medico = negocio.BuscarPorId(idMedico);

                if (medico == null)
                {
                    Session.Remove("IdMedicoEliminar");
                    Response.Redirect("GestionMedicos.aspx");
                    return;
                }

                lblNombre.Text = medico.Nombre;
                lblApellido.Text = medico.Apellido;
                // ✅ Especialidad es lista: mostramos la primera si existe
                lblEspecialidad.Text = (medico.Especialidad != null && medico.Especialidad.Any())
                    ? medico.Especialidad.First().Descripcion
                    : "Sin especialidad";
            }
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (Session["IdMedicoEliminar"] == null)
            {
                Response.Redirect("GestionMedicos.aspx");
                return;
            }

            int idMedico = (int)Session["IdMedicoEliminar"];
            MedicoNegocio negocio = new MedicoNegocio();

            try
            {
                negocio.Eliminar(idMedico); // ahora será eliminación lógica
                Session.Remove("IdMedicoEliminar");
                Response.Redirect("GestionMedicos.aspx");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    $"alert('Error al eliminar: {ex.Message}');", true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("IdMedicoEliminar");
            Response.Redirect("GestionMedicos.aspx");
        }
    }
}