using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;

namespace presentacion
{
    public partial class MiMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Seguridad para que si no hay una sesion activa, no se pueda acceder a las demas paginas, obliga a loguearte
            if (!(Page is LogIn) && !(Page is Registro))
            {
                if (!Seguridad.sessionActiva(Session["Usuario"]))
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            //Oculto NavBar en LogIn.aspx
            string currentPage = System.IO.Path.GetFileName(Request.Path);

            if (currentPage.Equals("LogIn.aspx", StringComparison.OrdinalIgnoreCase))
            {
                pnlNavbar.Visible = false;
            }

            if (Seguridad.sessionActiva(Session["Usuario"]))
            {
                //lnkLogin.Visible = false;
                btnSalir.Visible = true;
                /*lnkMenu.Visible = true;*/
                lnkSobreNosotros.Visible = true;

                if (Seguridad.esAdmin(Session["Usuario"]))
                {
                    lnkPacientes.Visible = true;
                    lnkMedicos.Visible = true;
                    lnkTurnos.Visible = true;
                    lnkMenu.Visible = true;
                    lnkEspecialidades.Visible = true;
                    lnkGestionMedicos.Visible = true;
                    lnkMenuPacientes.Visible = true;

                }
                else if (Seguridad.esMedico(Session["Usuario"]))
                {
                    lnkTurnos.Visible = true;
                }
                else if (Seguridad.esPaciente(Session["Usuario"]))
                {
                    lnkMenu.Visible = false;
                    lnkTurnos.Visible = false;
                    lnkTurnoPaciente.Visible = false;
                    lnkEspecialidades.Visible = true;
                    lnkMenuPacientes.Visible = true;
                    pnlTurnosPaciente.Visible = true;

                }
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx", false);
        }
    }

}