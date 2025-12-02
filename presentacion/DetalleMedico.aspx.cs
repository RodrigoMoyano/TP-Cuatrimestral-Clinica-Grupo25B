using System;
using System.Linq;
using System.Web.UI;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class DetalleMedico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["IdMedicoDetalle"] == null)
                {
                    Response.Redirect("GestionMedicos.aspx");
                    return;
                }

                int idMedico = (int)Session["IdMedicoDetalle"];

                MedicoNegocio negocio = new MedicoNegocio();
                Dominio.Medico medico = negocio.BuscarPorId(idMedico); // ✅ usamos Dominio.Medico

                if (medico == null)
                {
                    Session.Remove("IdMedicoDetalle");
                    Response.Redirect("GestionMedicos.aspx");
                    return;
                }

                lblId.Text = medico.Id.ToString();
                lblNombre.Text = medico.Nombre;
                lblApellido.Text = medico.Apellido;
                lblMatricula.Text = medico.Matricula;
                lblEmail.Text = medico.Email;
                lblTelefono.Text = medico.Telefono;

                // ✅ Mostrar todas las especialidades separadas por coma
                if (medico.Especialidad != null && medico.Especialidad.Any())
                    lblEspecialidad.Text = string.Join(", ", medico.Especialidad.Select(esp => esp.Descripcion));
                else
                    lblEspecialidad.Text = "Sin especialidades registradas";
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Session.Remove("IdMedicoDetalle");
            Response.Redirect("GestionMedicos.aspx");
        }
    }
}