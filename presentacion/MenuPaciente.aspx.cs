using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class MenuPaciente : PaginaPaciente
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnVerTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("TurnoPaciente.aspx");
        }

        protected void btnPedirTurno_Click(object sender, EventArgs e)
        {
            Response.Redirect("PedirTurno.aspx");
        }


        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }


    }
}
