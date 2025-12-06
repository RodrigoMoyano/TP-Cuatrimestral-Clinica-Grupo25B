using Dominio;
using Negocio;
using System;
using System.Web.Hosting;
using System.Web.UI;

namespace presentacion
{
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string msg = Request.QueryString["mensaje"];

                if(msg == "registrado")
                {
                    mensajeRegistro.Visible = true;
                    mensajeRegistro.InnerHtml = "✓ Paciente registrado exitosamente.";
                }
            }
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
                
                usuario.NombreUsuario = txtEmail.Text;
                usuario.Clave = txtPassword.Text;

                if (negocio.Login(usuario))
                {
                   
                    Session.Add("Usuario", usuario);

                    
                    if (usuario.Rol != null && usuario.Rol.Id == 3)   // 3 = Médico 
                    {
                        MedicoNegocio medNeg = new MedicoNegocio();
                 
                        var medico = medNeg.Listar().Find(m => m.IdUsuario == usuario.Id);

                        if (medico != null)
                        {
                            Session["IdMedico"] = medico.Id;
                        }
                    }

                   
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