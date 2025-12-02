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
                if (!Seguridad.sessionActiva(Session["usuario"]))
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            //Oculto NavBar en LogIn.aspx
            string currentPage = System.IO.Path.GetFileName(Request.Path);

            if (currentPage.Equals("LogIn.aspx", StringComparison.OrdinalIgnoreCase) || currentPage.Equals("Registro.aspx", StringComparison.OrdinalIgnoreCase))
            {
                pnlNavbar.Visible = false;
            }

            if (Seguridad.sessionActiva(Session["usuario"]))
            {
                //lnkLogin.Visible = false;
                btnSalir.Visible = true;

                if (Seguridad.esAdmin(Session["usuario"]))
                {
                    lnkMenu.Visible = true;
                    lnkMenuPacientes.Visible = false;

                    liAdmin.Visible = true;
                    lnkPacientes.Visible = false;
                    lnkMedicos.Visible = false;
                    lnkGestionMedicos.Visible = true;
                    lnkEspecialidades.Visible = true;

                    liTurnos.Visible = false;
                    lnkGestionTurnos.Visible= false;
                    
                    lnkMisTurnos.Visible = false;
                    lnkPedirTurno.Visible = false;


                }
                else if (Seguridad.esMedico(Session["usuario"]))
                {

                    lnkMenu.Visible = true;
                    lnkMenuPacientes.Visible = false;

                    liAdmin.Visible= false;

                    liTurnos.Visible = true;
                    lnkGestionTurnos.Visible = true;

                    lnkPanelMedico.Visible = true;

                    lnkMisTurnos.Visible = false;
                    lnkPedirTurno.Visible = false;

                    lnkSobreNosotros.Visible = false;
                }
                else if (Seguridad.esPaciente(Session["usuario"]))
                {
                    lnkMenu.Visible = true;
                    lnkMenuPacientes.Visible = true;

                    liAdmin.Visible = false;

                    liTurnos.Visible = false;
                    lnkGestionTurnos.Visible = false;

                    lnkMisTurnos.Visible = true;
                    lnkPedirTurno.Visible = true;

                    lnkSobreNosotros.Visible = true;
                }
                else
                {
                    liAdmin.Visible = false;
                    liTurnos.Visible = false;
                    btnSalir.Visible = false;
                    lnkMenu.Visible = false;
                    lnkMenuPacientes.Visible = false;
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