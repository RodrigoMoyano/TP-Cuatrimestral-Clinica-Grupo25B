using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class Perfil : PaginaPaciente
    {
        protected void Page_Load(object sender, EventArgs e)

        {
            if (!IsPostBack)
            {
                Usuario user = (Usuario)Session["usuario"];
                if (user == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                PacienteNegocio pacNeg = new PacienteNegocio();
                Paciente pac = pacNeg.ObtenerPorIdUsuario(user.Id);

                Session["paciente"] = pac;

                if (pac != null)
                    CargarDatosPerfil(pac.Id);

                
            }
        }
        private void CargarDatosPerfil(int id)
        {
            PacienteNegocio pacNeg = new PacienteNegocio();
            Paciente p = pacNeg.ObtenerPorId(id);
            if (p == null) return;

            if (p.Usuario != null)
            {
                txtUsuario.Text = p.Usuario.NombreUsuario;
                txtPassword.Attributes["value"] = p.Usuario.Clave ?? "";
            }

            txtNombre.Text = p.Nombre ?? "";
            txtApellido.Text = p.Apellido ?? "";
            txtDNI.Text = p.Dni ?? "";
            txtEmail.Text = p.Email ?? "";
            txtTelefono.Text = p.Telefono ?? "";

            
            if (p.Cobertura != null)
            {
                lblTipoCobertura.Text = p.Cobertura.Tipo;

                if (p.Cobertura.Tipo == "Obra Social")
                {
                    lblObraSocial.Text = $"{p.Cobertura.NombreObraSocial} {p.Cobertura.PlanCobertura}";
                }
                else
                {
                    lblObraSocial.Text = "-";
                }
            }
            else
            {
                lblTipoCobertura.Text = "Particular";
                lblObraSocial.Text = "-";
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            LimpiarErrores();

            bool error = false;

            Usuario usuarioSesion = (Usuario)Session["usuario"];
            UsuarioNegocio userNeg = new UsuarioNegocio();


            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                lblErrorUsuario.Text = "El nombre de usuario no puede estar vacío.";
                lblErrorUsuario.Visible = true;
                error = true;
            }
            else if (userNeg.NombreUsuarioExiste(txtUsuario.Text, usuarioSesion.Id))
            {
                lblErrorUsuario.Text = "El nombre de usuario ya existe.";
                lblErrorUsuario.Visible = true;
                error = true;
            }


            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblErrorEmail.Text = "Debe ingresar un email.";
                lblErrorEmail.Visible = true;
                error = true;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(
                        txtEmail.Text,
                        @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
                    ))
            {
                lblErrorEmail.Text = "Debe ingresar un email válido.";
                lblErrorEmail.Visible = true;
                error = true;
            }

            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                lblErrorTelefono.Text = "Debe ingresar un teléfono.";
                lblErrorTelefono.Visible = true;
                error = true;
            }
            else if (!txtTelefono.Text.All(char.IsDigit))
            {
                lblErrorTelefono.Text = "El teléfono solo puede contener números.";
                lblErrorTelefono.Visible = true;
                error = true;
            }
            else if (txtTelefono.Text.Length < 8)
            {
                lblErrorTelefono.Text = "El teléfono debe tener al menos 8 números.";
                lblErrorTelefono.Visible = true;
                error = true;
            }


            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblErrorPassword.Text = "La contraseña no puede estar vacía.";
                lblErrorPassword.Visible = true;
                error = true;
            }


            if (error)
                return;

            try
            {
                UsuarioNegocio negUser = new UsuarioNegocio();
                PacienteNegocio pacNeg = new PacienteNegocio();

                
                usuarioSesion.NombreUsuario = txtUsuario.Text;
                string nuevaClave = txtPassword.Text; 
                usuarioSesion.Clave = nuevaClave;

                negUser.ModificarPerfil(usuarioSesion);

                Paciente paciente = (Paciente)Session["paciente"];

                if (paciente == null)
                    throw new Exception("paciente vino NULL");

                paciente.Email = txtEmail.Text;
                paciente.Telefono = txtTelefono.Text;

               
                pacNeg.ModificarPerfilPaciente(paciente, nuevaClave);

                lblMensaje.CssClass = "text-success";
                lblMensaje.Text = "Datos actualizados correctamente.";
            }
            catch (Exception ex)
            {
                lblMensaje.CssClass = "text-danger";
                lblMensaje.Text = "Error al guardar: " + ex.Message;
            }
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("MenuPaciente.aspx");
        }
        private void LimpiarErrores()
        {
            lblErrorUsuario.Visible = false;
            lblErrorEmail.Visible = false;
            lblErrorTelefono.Visible = false;
            lblErrorPassword.Visible = false;
            lblMensaje.Text = "";
        }
    }
}