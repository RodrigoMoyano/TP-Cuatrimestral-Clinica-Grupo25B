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
            //if (!Seguridad.sessionActiva(Session["usuario"]))
            //{
            //    Response.Redirect("Login.aspx", true);
            //}
            //Usuario usuario = Session["usuario"] as Usuario;

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
        //Repogramar y cancelar turno
        protected void dgvVerTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reprogramar")
            {
                int idTurno = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("PedirTurno.aspx?id=" + idTurno);

            }
            if(e.CommandName == "Cancelar")
            {
                int idTurno = Convert.ToInt32(e.CommandArgument);

                TurnoNegocio negocio = new TurnoNegocio();
                negocio.CancelarTurno(idTurno);

                CargarListaTurnos();
            }
        }
        protected void dgvVerTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                VerTurno turno = (VerTurno)e.Row.DataItem;

                LinkButton btnCancelar = (LinkButton)e.Row.FindControl("btnCancelar");
                LinkButton btnReprogramar = (LinkButton)e.Row.FindControl("btnReprogramar");

                if (turno.Estado == "Cancelado" ||
                    turno.Estado == "No Asistió")
                {
                    btnCancelar.Visible = false;
                }
                if(turno.Estado == "Reprogramado")
                {
                    btnReprogramar.Visible = false;
                }
            }
        }

        //Filtro desplegable por estado del turno
        protected void ddlFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string estadoSeleccionado = ddlFiltroEstado.SelectedValue;

            List<VerTurno> lista = (List<VerTurno>)Session["SpVerTurno"];

            //Si no es nulo muestra por estado
            if (!string.IsNullOrEmpty(estadoSeleccionado))
            {
                List<VerTurno> listaFiltrada = lista.FindAll(x => x.Estado.Equals(estadoSeleccionado, StringComparison.OrdinalIgnoreCase));

                dgvVerTurnos.DataSource = listaFiltrada;
            }
            else
            { 
                //Me va a mostrar todo
                dgvVerTurnos.DataSource = lista;
            }
            dgvVerTurnos.DataBind();
        }

        protected void btnAgregarTurno_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgregarTurno.aspx");
        }

        protected void dgvVerTurnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvVerTurnos.PageIndex = e.NewPageIndex;
            CargarListaTurnos();
        }
    }
}