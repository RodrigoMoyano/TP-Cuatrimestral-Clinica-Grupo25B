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
                   
                    Session.Add("usuario", usuario);

                    
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

        protected void lnkOlvide_Click(object sender, EventArgs e)
        {
            Response.Redirect("OlvideContraseña.aspx", false);
        }
    }
}
