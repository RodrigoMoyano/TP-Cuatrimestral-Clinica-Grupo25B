using System;
using Dominio;
using Negocio;

namespace Presentacion
{
    public partial class ObservacionesTurno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int idTurno = Convert.ToInt32(Request.QueryString["id"]);

                TurnoNegocio turnoNegocio = new TurnoNegocio();
                Turno turno = turnoNegocio.BuscarPorId(idTurno);

                if (turno != null)
                {
                    lblFecha.Text = "Fecha: " + turno.Fecha.ToString("dd/MM/yyyy");

                    // ---- FORMATO DE HORA CORRECTO Y SEGURO ----
                    if (turno.Hora != null && TimeSpan.TryParse(turno.Hora.ToString(), out TimeSpan horaTS))
                        lblHora.Text = "Hora: " + horaTS.ToString(@"hh\:mm");
                    else
                        lblHora.Text = "Hora: -";

                    lblPaciente.Text = "Paciente ID: " + turno.Paciente.Id;
                    lblEspecialidad.Text = "Especialidad ID: " + turno.Especialidad.Id;
                    lblEstado.Text = "Estado ID: " + turno.Estado.Id;

                    lblObservaciones.Text = "Observaciones: " + turno.Observaciones;
                }
                else
                {
                    lblObservaciones.Text = "No se encontró el turno.";
                }
            }
        }

        // -----------------------------------------
        //   Mostrar modo edición
        // -----------------------------------------
        protected void btnEditarObs_Click(object sender, EventArgs e)
        {
            txtObservaciones.Visible = true;
            btnGuardarObs.Visible = true;
            btnCancelarEdicion.Visible = true;

            lblObservaciones.Visible = false;
            btnEditarObs.Visible = false;

            txtObservaciones.Text = lblObservaciones.Text.Replace("Observaciones: ", "");
        }

        // -----------------------------------------
        //   Guardar cambios
        // -----------------------------------------
        protected void btnGuardarObs_Click(object sender, EventArgs e)
        {
            int idTurno = Convert.ToInt32(Request.QueryString["id"]);
            TurnoNegocio negocio = new TurnoNegocio();

            negocio.ActualizarObservaciones(idTurno, txtObservaciones.Text);

            lblObservaciones.Text = "Observaciones: " + txtObservaciones.Text;

            txtObservaciones.Visible = false;
            btnGuardarObs.Visible = false;
            btnCancelarEdicion.Visible = false;

            lblObservaciones.Visible = true;
            btnEditarObs.Visible = true;
        }

        // -----------------------------------------
        //   Cancelar edición
        // -----------------------------------------
        protected void btnCancelarEdicion_Click(object sender, EventArgs e)
        {
            txtObservaciones.Visible = false;
            btnGuardarObs.Visible = false;
            btnCancelarEdicion.Visible = false;

            lblObservaciones.Visible = true;
            btnEditarObs.Visible = true;
        }

        // -----------------------------------------
        //   BOTONES EXISTENTES
        // -----------------------------------------
        protected void btnReprogramar_Click(object sender, EventArgs e)
        {
            int idTurno = Convert.ToInt32(Request.QueryString["id"]);
            new TurnoNegocio().CambiarEstado(idTurno, 2);
            Response.Redirect("PanelMedico.aspx");
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            int idTurno = Convert.ToInt32(Request.QueryString["id"]);
            new TurnoNegocio().CambiarEstado(idTurno, 5);
            Response.Redirect("PanelMedico.aspx");
        }

        protected void btnNoAsistio_Click(object sender, EventArgs e)
        {
            int idTurno = Convert.ToInt32(Request.QueryString["id"]);
            new TurnoNegocio().CambiarEstado(idTurno, 4);
            Response.Redirect("PanelMedico.aspx");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            int idTurno = Convert.ToInt32(Request.QueryString["id"]);
            new TurnoNegocio().CambiarEstado(idTurno, 3);
            Response.Redirect("PanelMedico.aspx");
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("PanelMedico.aspx");
        }
    }
}