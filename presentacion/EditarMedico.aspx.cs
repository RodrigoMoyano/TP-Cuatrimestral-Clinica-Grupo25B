using Dominio;
using Negocio;
using System;
using System.Linq;
using System.Web.UI;

namespace presentacion
{
    public partial class EditarMedico : System.Web.UI.Page
    {
        private int idMedico;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;

            if (Session["IdMedicoEditar"] == null)
            {
                Response.Redirect("GestionMedicos.aspx");
                return;
            }

            idMedico = (int)Session["IdMedicoEditar"];

            if (!IsPostBack)
            {
                CargarEspecialidades();
                CargarMedico();
            }
        }

        private void CargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            ddlEspecialidad.DataSource = negocio.Listar();
            ddlEspecialidad.DataValueField = "Id";
            ddlEspecialidad.DataTextField = "Descripcion";
            ddlEspecialidad.DataBind();
        }

        private void CargarMedico()
        {
            MedicoNegocio negocio = new MedicoNegocio();
            Dominio.Medico med = negocio.BuscarPorId(idMedico);

            if (med == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('El médico no existe.');", true);
                Response.Redirect("GestionMedicos.aspx");
                return;
            }

            txtNombre.Text = med.Nombre;
            txtApellido.Text = med.Apellido;
            txtMatricula.Text = med.Matricula;
            txtTelefono.Text = med.Telefono;
            txtEmail.Text = med.Email;

            // ✅ Seleccionamos la primera especialidad si existe
            if (med.Especialidad != null && med.Especialidad.Any())
                ddlEspecialidad.SelectedValue = med.Especialidad.First().Id.ToString();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Dominio.Medico med = new Dominio.Medico
                {
                    Id = idMedico,
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Matricula = txtMatricula.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Especialidad = new System.Collections.Generic.List<Especialidad>
                    {
                        new Especialidad { Id = int.Parse(ddlEspecialidad.SelectedValue) }
                    }
                };

                MedicoNegocio negocio = new MedicoNegocio();
                negocio.Modificar(med);

                Session.Remove("IdMedicoEditar");
                Response.Redirect("GestionMedicos.aspx");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    $"alert('Error al guardar: {ex.Message}');", true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("IdMedicoEditar");
            Response.Redirect("GestionMedicos.aspx");
        }
    }
}