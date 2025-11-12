using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class MiMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Oculto NavBar en LogIn.aspx
            string currentPage = System.IO.Path.GetFileName(Request.Path);

            if(currentPage.Equals("LogIn.aspx", StringComparison.OrdinalIgnoreCase))
            {
                pnlNavbar.Visible = false;
            }

            if (!IsPostBack)
            {
                if (Session["usuario"] != null)
                {
                    Usuario usuario = (Usuario)Session["usuario"];

                    if (usuario.Rol.Descripcion == "Paciente")
                    {
                        lnkInicio.HRef = "MenuPaciente.aspx";
                        lnkEspecialidades.Visible = true;
                        lnkSobreNosotros.Visible = true;
                        lnkSalir.Visible = true;

                        // Ocultamos lo demás
                        lnkPacientes.Visible = false;
                        lnkMedicos.Visible = false;
                        lnkTurnos.Visible = false;
                    }
                    else if (usuario.Rol.Descripcion == "Administrador")
                    {
                        // Ejemplo: el admin ve todo
                        lnkInicio.HRef = "InicioAdmin.aspx";
                        lnkEspecialidades.Visible = true;
                        lnkSobreNosotros.Visible = true;
                        lnkSalir.Visible = true;
                        lnkPacientes.Visible = true;
                        lnkMedicos.Visible = true;
                        lnkTurnos.Visible = true;
                    }
                }
                else
                {
                    // Usuario no logueado
                    lnkPacientes.Visible = false;
                    lnkMedicos.Visible = false;
                    lnkTurnos.Visible = false;
                    lnkEspecialidades.Visible = true;
                    lnkSobreNosotros.Visible = true;
                    lnkSalir.Visible = false;
                }
            }
        }
    }

}