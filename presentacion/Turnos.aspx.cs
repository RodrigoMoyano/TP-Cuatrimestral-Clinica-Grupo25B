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
    public partial class Turnos : PaginaAdmin
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.sessionActiva(Session["usuario"]))
            {
                Response.Redirect("Login.aspx", true);
            }

            if (!IsPostBack)
            {
                CargarListaTurnos();
            }
        }
        private void CargarListaTurnos()
        {
            TurnoNegocio negocio = new TurnoNegocio();

            try
            {
                List<VerTurno> lista = negocio.spVerTurno();

                Session.Add("SpVerTurno", lista);
                dgvVerTurnos.DataSource = Session["SpVerTurno"];
                dgvVerTurnos.DataBind();

                //dgvVerTurnos.DataSource = lista;
                //dgvVerTurnos.DataBind();
            }
            catch (Exception ex)
            {
                Usuario usuario = new Usuario();

                throw ex;
            }


        }

        protected void txtfiltro_TextChanged(object sender, EventArgs e)
        {
            List<VerTurno> list = (List<VerTurno>)Session["SpVerTurno"];
            List<VerTurno> listaFiltrada = list.FindAll(x => x.Paciente.ToUpper().Contains(txtfiltro.Text.ToUpper()));
            dgvVerTurnos.DataSource = listaFiltrada;
            dgvVerTurnos.DataBind();
        }

        protected void dgvVerTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reprogramar")
            {
                int idTurno = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("PedirTurno.aspx?id=" + idTurno);

            }
        }

        //protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ddlCriterio.Items.Clear();
        //    if (ddlCampo.SelectedIndex.ToString() == "Numero") 
        //    {
        //        ddlCriterio.Items.Add("Igual a");
        //        ddlCriterio.Items.Add("Mayor a");
        //        ddlCriterio.Items.Add("Menor a");
        //    }
        //    else
        //    {
        //        ddlCriterio.Items.Add("Contiene");
        //        ddlCriterio.Items.Add("Comienza con");
        //        ddlCriterio.Items.Add("Termina con");
        //    }
        //}
    }
}