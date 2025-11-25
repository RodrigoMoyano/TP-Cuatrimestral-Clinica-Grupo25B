using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class Turnos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            /*if(!IsPostBack)
            {
                CargarListaTurnos();
            }*/
        }
        /*private void CargarListaTurnos()
        {
            TurnoNegocio negocio = new TurnoNegocio();

            try
            {
                List<VerTurno> lista =  negocio.spVerTurno();

                dgvVerTurno.DataSource = lista;
                dgvVerTurno.DataBind();
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }*/
    }
}