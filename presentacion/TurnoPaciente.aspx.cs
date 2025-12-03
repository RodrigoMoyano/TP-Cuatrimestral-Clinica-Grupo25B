using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class TurnoPaciente : PaginaPaciente
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!VerificarSession())
                {
                    return;
                }

                CargarTurnos();
            }
        }

        private bool VerificarSession()
        {
            Usuario usuario = Session["usuario"] as Usuario;
            if(usuario == null)
            {
                Response.Redirect("Login.aspx", false);
                return false;
            }
            return true;
        }

        private void CargarTurnos()
        {
            try
            {
                Usuario usuario = (Usuario)Session["usuario"];

                if(usuario == null)
                {
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                PacienteNegocio pacNeg = new PacienteNegocio();
                int idPaciente = pacNeg.ObtenerIdPacientePorIdUsuario(usuario.Id);

                TurnoNegocio turnoNeg = new TurnoNegocio();

                string estadoSeleccionado = ddlFiltroEstado.SelectedValue;

                List<Turno> lista;

                if (string.IsNullOrEmpty(estadoSeleccionado))
                {
                    
                    lista = turnoNeg.ListarPorPaciente(idPaciente);
                }
                else
                {
                    
                    lista = turnoNeg.ListarPorPacienteFiltrado(idPaciente, estadoSeleccionado);
                }

                gvTurnos.DataSource = lista;
                gvTurnos.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar turnos: " + ex.Message);
            }
        }
        protected void gvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancelar")
            {
                int idTurno = Convert.ToInt32(e.CommandArgument);

                TurnoNegocio negocio = new TurnoNegocio();
                negocio.Cancelar(idTurno);


                CargarTurnos();
            }
        }
        //Para que el boton cancelar desaparezca 
        protected void gvTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Turno turno = (Turno)e.Row.DataItem;


                Button btnCancelar = (Button)e.Row.FindControl("btnCancelar");


                if (turno.Estado.Descripcion == "Cancelado" ||
                    turno.Estado.Descripcion == "No Asistió")
                {
                    btnCancelar.Visible = false;
                }
            }
        }
        protected void ddlFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTurnos();
        }

    }
}