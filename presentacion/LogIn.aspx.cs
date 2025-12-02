using Dominio;
using Negocio;
using System;
using System.Web.UI;

namespace presentacion
{
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
                return;

            Usuario usuario = new Usuario();
            UsuarioNegocio negocio = new UsuarioNegocio();

            try
            {
                // Estos controles siguen igual
                usuario.NombreUsuario = txtEmail.Text;
                usuario.Clave = txtPassword.Text;

                if (negocio.Login(usuario))
                {
                    // ✅ 1) guardo el usuario en sesión (lo que ya tenías)
                    Session.Add("Usuario", usuario);

                    // ✅ 2) si el usuario es MÉDICO, obtengo su IdMedico y lo guardo en sesión
                    if (usuario.Rol != null && usuario.Rol.Id == 3)   // 3 = Médico en tu tabla Rol
                    {
                        MedicoNegocio medNeg = new MedicoNegocio();
                        // Uso Listar() y busco el médico cuyo IdUsuario coincida
                        var medico = medNeg.Listar().Find(m => m.IdUsuario == usuario.Id);

                        if (medico != null)
                        {
                            Session["IdMedico"] = medico.Id;
                        }
                    }

                    // ✅ 3) redirección como antes (luego desde el menú vas a PanelMedico)
                    Response.Redirect("Menu.aspx", false);
                }
                else
                {
                    lblError.Text = "Error: Usuario o Contraseña incorrectos";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
                throw;
            }
        }

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registro.aspx", false);
        }
    }
}
/*namespace presentacion
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

            UsuarioNegocio usuarioNeg = new UsuarioNegocio();
            PacienteNegocio pacienteNeg = new PacienteNegocio();

            try
            {
                Usuario user = usuarioNeg.ObtenerUsuario(usuario, clave);

                if (user != null)
                {
                    Session["Usuario"] = user;
                    string rol = user.Rol.Descripcion;

                    switch (rol)
                    {
                        case "Paciente":
                            // Obtener el IdPaciente por IdUsuario
                            int idPaciente = pacienteNeg.ObtenerIdPacientePorIdUsuario(user.Id);

                            if (idPaciente <= 0)
                            {
                                lblError.Visible = true;
                                lblError.Text = "No se encontró un paciente asociado a este usuario.";
                                return;
                            }

                            Session["IdPaciente"] = idPaciente;
                            Response.Redirect("MenuPaciente.aspx", false);
                            break;

                        case "Medico":
                            Response.Redirect("MenuMedico.aspx", false);
                            break;


                        case "Admin":
                            Response.Redirect("Menu.aspx", false);
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


        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registro.aspx", false);

        }
    }
}*/