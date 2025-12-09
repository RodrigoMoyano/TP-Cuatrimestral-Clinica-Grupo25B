using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.WebSockets;

namespace presentacion
{
    public partial class AgregarTurno : PaginaAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarPaciente();
                cargarEspecialidades();
            }
        }
        private void cargarPaciente()
        {
            PacienteNegocio negocio = new PacienteNegocio();
            ddlPacientes.DataSource = negocio.ObtenerTodos();
            ddlPacientes.DataTextField = "ApellidoNombre";
            ddlPacientes.DataValueField = "Id";
            ddlPacientes.DataBind();
        }
        private void cargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            ddlEspecialidades.DataSource = negocio.Listar();
            ddlEspecialidades.DataTextField = "Descripcion";
            ddlEspecialidades.DataValueField= "Id";
            ddlEspecialidades.DataBind();
        }

        protected void ddlEspecialidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idEspecialidad = int.Parse(ddlEspecialidades.SelectedValue);

            MedicoNegocio negocio = new MedicoNegocio();
            ddlMedicos.DataSource = negocio.ListarPorEspecialidad(idEspecialidad);
            ddlMedicos.DataTextField = "NombreCompleto";
            ddlMedicos .DataValueField = "Id";
            ddlMedicos .DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Turno turno = new Turno();
            TurnoNegocio negocio = new TurnoNegocio();

            turno.Paciente = new Paciente();
            turno.Paciente.Id = int.Parse(ddlPacientes.SelectedValue);

            turno.Medico = new Medico();
            turno.Medico.Id = int.Parse(ddlMedicos.SelectedValue);

            turno.Especialidad = new Especialidad();
            turno.Especialidad.Id = int.Parse(ddlEspecialidades.SelectedValue);
            
            turno.Fecha = calFecha.SelectedDate;
            turno.Hora = TimeSpan.Parse(ddlHoras.SelectedValue);

            turno.Estado = new EstadoTurno();
            turno.Estado.Id = 1;
            turno.Estado.Descripcion = "Nuevo";

            negocio.Agregar(turno);

            Response.Redirect("Turnos.aspx");

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void ddlMedicos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlHoras.Items.Clear();
            calFecha.SelectedDates.Clear();
        }
        protected void calFecha_DayRender(object sender, DayRenderEventArgs e)
        {
            if(string.IsNullOrEmpty(ddlMedicos.SelectedValue))
            {
                return;
            }

            int idMedico;
            if(!int.TryParse(ddlMedicos.SelectedValue, out idMedico))
            {
                return;
            }

            string diaSemana = e.Day.Date.ToString("dddd", new CultureInfo("es-ES"));
            diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);

            TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();
            var turno = negocio.ObtenerHorario(idMedico, diaSemana);

            if(turno != null)
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        protected void calFecha_SelectionChanged(object sender, EventArgs e)
        {
            ddlHoras.Items.Clear ();

            DateTime fechaSelecionada = calFecha.SelectedDate;

            if(fechaSelecionada < DateTime.Now.Date)
            {
                calFecha.SelectedDates.Clear();
                return;
            }

            if(ddlMedicos.SelectedValue == "")
            {
                return;
            }

            int idMedico = int.Parse(ddlMedicos.SelectedValue);

            string diaSemana = fechaSelecionada.ToString("dddd", new CultureInfo("es-ES") );
            diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);

            TurnoTrabajoNegocio trabajoNegocio = new TurnoTrabajoNegocio();
            var turnoTrabajo = trabajoNegocio.ObtenerHorario(idMedico, diaSemana);

            if(turnoTrabajo == null)
            {
                return;
            }

            TurnoNegocio turnosNegocio = new TurnoNegocio();
            List<TimeSpan> ocupados = turnosNegocio.ObtenerHorariosOcupados(idMedico, fechaSelecionada);

            TimeSpan inicio = turnoTrabajo.HoraInicio;
            TimeSpan fin = turnoTrabajo.HoraFin;

            for(TimeSpan h = inicio; h < fin; h = h.Add(TimeSpan.FromMinutes(30)))
            {
                if (fechaSelecionada.Date == DateTime.Now.Date && h <= DateTime.Now.TimeOfDay)
                {
                    continue;
                }

                if(!ocupados.Contains(h))
                {
                    ddlHoras.Items.Add(h.ToString(@"hh\:mm"));
                }
            }
        }
    }
}