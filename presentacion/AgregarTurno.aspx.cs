using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.WebSockets;

namespace presentacion
{
    public partial class AgregarTurno : PaginaAdmin
    {
        private string observaciones;

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
            ddlEspecialidades.DataValueField = "Id";
            ddlEspecialidades.DataBind();

            ddlEspecialidades.Items.Insert(0, new ListItem("-- Seleccione especialidad --", ""));
        }

        protected void ddlEspecialidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlEspecialidades.SelectedValue))
            {
                ddlMedicos.Items.Clear();
                ddlMedicos.Items.Insert(0, new ListItem("-- Seleccione médico --", ""));
                
                calFecha.SelectedDates.Clear();
                ddlHoras.Items.Clear();
                calFecha.VisibleDate = DateTime.Now;
                calFecha.DataBind();
                return;
            }

            int idEspecialidad = int.Parse(ddlEspecialidades.SelectedValue);

            MedicoNegocio negocio = new MedicoNegocio();
            ddlMedicos.DataSource = negocio.ListarPorEspecialidad(idEspecialidad);
            ddlMedicos.DataTextField = "NombreCompleto";
            ddlMedicos.DataValueField = "Id";
            ddlMedicos.DataBind();

            ddlMedicos.Items.Insert(0, new ListItem("-- Seleccione médico --", ""));

            calFecha.SelectedDates.Clear();
            ddlHoras.Items.Clear();
            calFecha.VisibleDate = DateTime.Now;
            calFecha.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                TurnoNegocio negocio = new TurnoNegocio();

                int idPaciente = int.Parse(ddlPacientes.SelectedValue);
                int idMedico = int.Parse(ddlMedicos.SelectedValue);
                int idEspecialidad = int.Parse(ddlEspecialidades.SelectedValue);

                DateTime fecha = calFecha.SelectedDate;
                TimeSpan hora = TimeSpan.Parse(ddlHoras.SelectedValue);

                string observaciones = txtObservaciones.Text.Trim();

                //Guardo el turno
                negocio.Agregar(idPaciente, idMedico, idEspecialidad, fecha, hora, observaciones);

                //datos del paciente
                PacienteNegocio pacNegocio = new PacienteNegocio();
                var paciente = pacNegocio.ObtenerPorId(idPaciente);

                string emailDestino = paciente.Email;
                string nombrePaciente = paciente.Nombre;
                string medicoNombre = ddlMedicos.SelectedItem.Text;

                //Envio email
                EmailService emailService = new EmailService();
                emailService.EnviarConfirmacionTurno(emailDestino, nombrePaciente, fecha, medicoNombre);

                
                Response.Redirect("Turnos.aspx");
            }
            catch (SmtpException smtpEx)
            {
                lblError.Text = "Error al enviar el correo: " + smtpEx.Message;
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Turnos.aspx");
        }

        protected void ddlMedicos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlHoras.Items.Clear();
            calFecha.SelectedDates.Clear();
            
            calFecha.VisibleDate = DateTime.Now;

            calFecha.TodaysDate = DateTime.Now;

            calFecha.DataBind();
        }
        protected void calFecha_DayRender(object sender, DayRenderEventArgs e)
        {
            if (string.IsNullOrEmpty(ddlMedicos.SelectedValue))
            {
                return;
            }

            int idMedico;
            if (!int.TryParse(ddlMedicos.SelectedValue, out idMedico))
            {
                return;
            }

            string diaSemana = e.Day.Date.ToString("dddd", new CultureInfo("es-ES"));
            diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);

            TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();
            var turno = negocio.ObtenerHorario(idMedico, diaSemana);

            if (turno != null)
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        protected void calFecha_SelectionChanged(object sender, EventArgs e)
        {
            ddlHoras.Items.Clear();

            DateTime fechaSelecionada = calFecha.SelectedDate;

            if (fechaSelecionada < DateTime.Now.Date)
            {
                calFecha.SelectedDates.Clear();
                return;
            }

            if (ddlMedicos.SelectedValue == "")
            {
                return;
            }

            int idMedico = int.Parse(ddlMedicos.SelectedValue);

            string diaSemana = fechaSelecionada.ToString("dddd", new CultureInfo("es-ES"));
            diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);

            TurnoTrabajoNegocio trabajoNegocio = new TurnoTrabajoNegocio();
            var turnoTrabajo = trabajoNegocio.ObtenerHorario(idMedico, diaSemana);

            if (turnoTrabajo == null)
            {
                return;
            }

            TurnoNegocio turnosNegocio = new TurnoNegocio();
            List<TimeSpan> ocupados = turnosNegocio.ObtenerHorariosOcupados(idMedico, fechaSelecionada);

            TimeSpan inicio = turnoTrabajo.HoraInicio;
            TimeSpan fin = turnoTrabajo.HoraFin;

            for (TimeSpan h = inicio; h < fin; h = h.Add(TimeSpan.FromMinutes(30)))
            {
                if (fechaSelecionada.Date == DateTime.Now.Date && h <= DateTime.Now.TimeOfDay)
                {
                    continue;
                }

                if (!ocupados.Contains(h))
                {
                    ddlHoras.Items.Add(h.ToString(@"hh\:mm"));
                }
            }
        }

        protected void cvFecha_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = calFecha.SelectedDate != DateTime.MinValue;
        }
    }
}