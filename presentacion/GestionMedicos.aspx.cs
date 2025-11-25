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
    public partial class GestionMedicos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrilla();
                CargarEspecialidades();
            }
        }
        private void CargarGrilla()
        {
            try
            {
                MedicoNegocio negocio = new MedicoNegocio();
                gvMedicos.DataSource = negocio.Listar();
                gvMedicos.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert", $"alert('Error: {ex.Message}');", true);
            }
        }
        private void CargarEspecialidades()
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("SELECT Id, Descripcion FROM Especialidad ORDER BY Descripcion");
                    datos.EjecutarLectura();

                    ddlEspecialidad.DataSource = datos.Lector;
                    ddlEspecialidad.DataTextField = "Descripcion";
                    ddlEspecialidad.DataValueField = "Id";
                    ddlEspecialidad.DataBind();
                }
                finally
                {
                    datos.CerrarConexion();
                }
            }

            ddlEspecialidad.Items.Insert(0, new ListItem("-- Seleccione una especialidad --", "0"));
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Dominio.Medico nuevo = new Dominio.Medico
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Matricula = "", // campo opcional
                    Email = txtEmail.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    IdUsuario = int.Parse(ddlEspecialidad.SelectedValue)
                };

                MedicoNegocio negocio = new MedicoNegocio();
                negocio.Agregar(nuevo);

                // Refrescar la grilla
                CargarGrilla();

                // Limpiar formulario
                txtNombre.Text = txtApellido.Text = txtEmail.Text = txtTelefono.Text = "";
                ddlEspecialidad.SelectedIndex = 0;
                ddlEstado.SelectedIndex = 0;

                // Mensaje de confirmación
                ScriptManager.RegisterStartupScript(this, GetType(), "successAlert", "alert('Médico agregado correctamente');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert", $"alert('Error: {ex.Message}');", true);
            }
        }
    }
}
    
