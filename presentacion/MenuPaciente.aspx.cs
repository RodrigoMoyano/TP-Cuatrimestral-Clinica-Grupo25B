using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class MenuPaciente : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnVerTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("TurnosPaciente.aspx");
        }

        protected void btnPedirTurno_Click(object sender, EventArgs e)
        {
            Response.Redirect("PedirTurno.aspx");
        }

        protected void btnCancelarTurno_Click(object sender, EventArgs e)
        {
            Response.Redirect("CancelarTurno.aspx");
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }


    }
}
