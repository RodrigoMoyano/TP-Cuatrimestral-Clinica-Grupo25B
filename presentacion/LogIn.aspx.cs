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
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtEmail.Text.Trim();
            string clave = txtPassword.Text.Trim();

            UsuarioNegocio negocio = new UsuarioNegocio();

            try
            {
                // Buscar usuario en la base
                Usuario user = negocio.ObtenerUsuario(usuario, clave);

                if (user != null)
                {
                    Session["Usuario"] = user;

                    // Redirige según el rol del usuario
                    switch (user.Rol.Descripcion.ToLower())
                    {
                        case "paciente":
                            Response.Redirect("MenuPaciente.aspx", false);
                            break;

                        case "medico":
                            Response.Redirect("MenuMedico.aspx", false);
                            break;

                        case "administrador":
                        case "admin":
                            Response.Redirect("MenuAdministrador.aspx", false);
                            break;

                        default:
                            lblError.Visible = true;
                            lblError.Text = "Rol de usuario no reconocido.";
                            break;
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Usuario o contraseña incorrectos.";
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error al iniciar sesión: " + ex.Message;
            }
        }
    }
}