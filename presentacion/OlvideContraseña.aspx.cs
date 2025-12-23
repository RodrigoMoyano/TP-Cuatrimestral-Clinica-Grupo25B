using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class OlvideContraseña : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            string usuarioIngresado = txtUsuario.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuarioIngresado))
            {
                lblMensaje.Text = "Ingrese un nombre de usuario.";
                return;
            }

            UsuarioNegocio userNeg = new UsuarioNegocio();
            PacienteNegocio pacNeg = new PacienteNegocio();

            // Buscar usuario
            Usuario u = userNeg.ObtenerPorNombreUsuario(usuarioIngresado);

            if (u == null)
            {
                lblMensaje.Text = "El usuario no existe.";
                return;
            }

            // Obtener correo asociado
            string emailDestino = pacNeg.ObtenerEmailPorIdUsuario(u.Id);

            if (string.IsNullOrEmpty(emailDestino))
            {
                lblMensaje.Text = "El usuario no tiene un correo asociado.";
                return;
            }

            // Contraseña en texto plano
            try
            {
                EmailService servicio = new EmailService();

                servicio.EnviarContraseñaOlvidada(emailDestino, u.NombreUsuario, u.Clave);

                lblMensaje.CssClass = "Correcto.";
                lblMensaje.Text = "Listo! Contraseña enviada a tu correo.";
            }
            catch (Exception ex)
            {

                lblMensaje.CssClass = "Error";
                lblMensaje.Text = "Error al enviar correo: " + ex.Message;
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}