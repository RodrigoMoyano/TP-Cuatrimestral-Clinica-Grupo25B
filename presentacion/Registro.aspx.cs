using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDesplegables();
            }
        }

        private void CargarDesplegables()
        {
            try
            {
                CoberturaNegocio negocio = new CoberturaNegocio();
                ddlCobertura.DataSource = negocio.Listar();
                ddlCobertura.DataTextField = "NombreObraSocial";
                ddlCobertura.DataValueField = "Id";
                ddlCobertura.DataBind();

                ddlCobertura.Items.Insert(0, new ListItem("Seleccione una cobertura...", "0"));

            }
            catch (Exception ex)
            {

                lblMensaje.Text = "Error al cargar coberturas: " + ex.Message;
            }
        }
        protected void btnAceptarRegistro_Click(object sender, EventArgs e)
        {
            //PAra limpiar el mensaje de error
            lblMensaje.Text = "";

            try
            {
                //Validacion de errores de Pagina
                Page.Validate();
                if (!Page.IsValid)
                {
                    return;
                }

                //Instancia Usuario
                Usuario user = new Usuario();
                user.NombreUsuario = txtNombreUsuario.Text;
                user.Clave = txtPassword.Text;
                user.Activo = true;
                //Instancia Pacientes
                Paciente paciente = new Paciente();
                paciente.Nombre = txtNombre.Text;
                paciente.Apellido = txtApellido.Text;
                paciente.Dni = txtDni.Text;
                paciente.Email = txtEmail.Text;
                paciente.Telefono = txtTelefono.Text;

                //Compruebo que el usuario no exista
                UsuarioNegocio negocioUsuario = new UsuarioNegocio();
                if (negocioUsuario.ExisteUsuario(user.NombreUsuario))
                {
                    lblMensaje.Text = "Error: El usuario ya fue registrado.";
                    return;
                }

                //compruebo que el email no exista
                PacienteNegocio negocioPaciente = new PacienteNegocio();
                if (negocioPaciente.ExisteCorreo(txtEmail.Text))
                {
                    lblMensaje.Text = "Error: El correo ya fue registrado.";
                    return;
                }

                //Cobertura
                if (int.TryParse(ddlCobertura.SelectedValue, out int idCobertura) && idCobertura > 0)
                {
                    paciente.Cobertura = new Cobertura { Id = idCobertura };
                }
                else
                {
                    lblMensaje.Text = "Selecciona una cobertura valida.";
                    return;
                }

                
                paciente.Usuario = user;

                //llamado al metodo con todos los objetos
                negocioPaciente.RegistrarPaciente(paciente);

                Response.Redirect("Login.aspx?mensaje=registrado", false);
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al registrar paciente: " + ex.InnerException.Message;
            }
        }
    }
}
