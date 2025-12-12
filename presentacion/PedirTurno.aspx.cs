using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class PedirTurno : PaginaPaciente
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario usuario = (Usuario)Session["usuario"];
            PacienteNegocio pacNeg = new PacienteNegocio();
            Paciente paciente = pacNeg.ObtenerPorIdUsuario(usuario.Id);


            if (!IsPostBack)
            {
                
                
                if (Request.QueryString["id"] != null)
                {
                    int idTurno = int.Parse(Request.QueryString["id"]);

                    ViewState["IdTurnoEditar"] = idTurno;

                    CargarEspecialidades();
                    CargarDatosDelTurno(idTurno);

                    ddlEspecialidad.Enabled = false;
                    ddlMedico.Enabled = false;

                    
                    TurnoNegocio turnoNeg = new TurnoNegocio();
                    Turno turno = turnoNeg.BuscarPorId(idTurno);

                    
                    Paciente pacienteTurno = pacNeg.ObtenerPorId(turno.Paciente.Id);

                    if (pacienteTurno != null)
                        CargarCoberturaPaciente(pacienteTurno);

                    return;
                }

                
                CargarEspecialidades();

                if (paciente == null)
                {
                    MostrarError("No se pudo cargar el perfil del paciente.");
                    return;
                }

                Session["paciente"] = paciente;
                CargarCoberturaPaciente(paciente);
            }
        }

        
            
        private void CargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            ddlEspecialidad.DataSource = negocio.Listar();
            ddlEspecialidad.DataTextField = "Descripcion";
            ddlEspecialidad.DataValueField = "Id";
            ddlEspecialidad.DataBind();
            ddlEspecialidad.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
        }

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMedico.Items.Clear();
            int idEspecialidad = int.Parse(ddlEspecialidad.SelectedValue);
            if (idEspecialidad == 0) return;

            MedicoNegocio mNeg = new MedicoNegocio();
            var medicos = mNeg.ListarPorEspecialidad(idEspecialidad);

            ddlMedico.DataSource = medicos;
            ddlMedico.DataTextField = "NombreCompleto";
            ddlMedico.DataValueField = "Id";
            ddlMedico.DataBind();
            ddlMedico.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
        }

        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlHorario.Items.Clear();
            calFecha.SelectedDates.Clear();
        }
        
        private void CargarCoberturaPaciente(Paciente p)
        {
            if (p.Cobertura != null)
            {
                lblTipoCobertura.Text = p.Cobertura.Tipo;

                if (p.Cobertura.Tipo == "Obra Social")
                {
                    lblObraSocial.Text = $"{p.Cobertura.NombreObraSocial} {p.Cobertura.PlanCobertura}";
                }
                else
                {
                    lblObraSocial.Text = "-";
                }
            }
            else
            {
                lblTipoCobertura.Text = "Particular";
                lblObraSocial.Text = "-";
            }
        }


        protected void calFecha_SelectionChanged(object sender, EventArgs e)
        {
            ddlHorario.Items.Clear();
            ddlHorario.Items.Clear();

            DateTime fechaSeleccionada = calFecha.SelectedDate;


            if (fechaSeleccionada.Date < DateTime.Now.Date)
            {
                lblErrorFecha.Text = "No se puede seleccionar una fecha pasada.";
                lblErrorFecha.Visible = true;

                calFecha.SelectedDates.Clear();
                return;
            }
            else
            {
                lblErrorFecha.Visible = false;
            }

            if (ddlMedico.SelectedValue == "0")
                return;

            int idMedico = int.Parse(ddlMedico.SelectedValue);


            string diaSemana = fechaSeleccionada.ToString("dddd", new CultureInfo("es-ES"));

            TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();
            var turno = negocio.ObtenerHorario(idMedico, diaSemana);

            if (turno == null)
                return;


            TurnoNegocio turnoNeg = new TurnoNegocio();
            List<TimeSpan> horariosOcupados = turnoNeg.ObtenerHorariosOcupados(idMedico, fechaSeleccionada);


            TimeSpan inicio = turno.HoraInicio;
            TimeSpan fin = turno.HoraFin;

            for (TimeSpan hora = inicio; hora < fin; hora = hora.Add(TimeSpan.FromMinutes(30)))
            {
                if (fechaSeleccionada.Date == DateTime.Now.Date &&
                    hora <= DateTime.Now.TimeOfDay)
                    continue;

                if (!horariosOcupados.Contains(hora))
                {
                    ddlHorario.Items.Add(hora.ToString(@"hh\:mm"));
                }

            }
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            LimpiarErrores();

            
            bool hayError = false;

            if (ddlEspecialidad.SelectedValue == "0")
            {
                lblErrorEspecialidad.Text = "Debe seleccionar una especialidad.";
                lblErrorEspecialidad.Visible = true;
                hayError = true;
            }

            if (ddlMedico.SelectedValue == "0")
            {
                lblErrorMedico.Text = "Debe seleccionar un médico.";
                lblErrorMedico.Visible = true;
                hayError = true;
            }

            if (calFecha.SelectedDate == DateTime.MinValue)
            {
                lblErrorFecha.Text = "Debe seleccionar una fecha.";
                lblErrorFecha.Visible = true;
                hayError = true;
            }

            if (ddlHorario.SelectedValue == "")
            {
                lblErrorHorario.Text = "Debe seleccionar un horario.";
                lblErrorHorario.Visible = true;
                hayError = true;
            }

           
            if (hayError)
                return;

            
            int idMedico = int.Parse(ddlMedico.SelectedValue);
            int idEspecialidad = int.Parse(ddlEspecialidad.SelectedValue);
            DateTime fecha = calFecha.SelectedDate;
            TimeSpan hora = TimeSpan.Parse(ddlHorario.SelectedValue);
            string observaciones = txtObservaciones.Text.Trim();

            
            if (fecha.Date < DateTime.Now.Date)
            {
                MostrarError("No se puede seleccionar una fecha pasada.");
                return;
            }

            if (fecha.Date == DateTime.Now.Date && hora <= DateTime.Now.TimeOfDay)
            {
                MostrarError("No se puede seleccionar una hora que ya pasó.");
                return;
            }

            try
            {
                TurnoNegocio negocio = new TurnoNegocio();

                
                if (ViewState["IdTurnoEditar"] != null)
                {
                    int idTurno = (int)ViewState["IdTurnoEditar"];

                    negocio.Modificar(idTurno, idMedico, idEspecialidad, fecha, hora, observaciones);

                    ViewState["IdTurnoEditar"] = null;

                    Response.Redirect("Turnos.aspx");
                    return;
                }

                
                Usuario usuario = (Usuario)Session["usuario"];
                PacienteNegocio pacNeg = new PacienteNegocio();
                int idPaciente = pacNeg.ObtenerIdPacientePorIdUsuario(usuario.Id);

                if (idPaciente == 0)
                {
                    MostrarError("Error: No se encontró el perfil del paciente.");
                    return;
                }


                negocio.Agregar(idPaciente, idMedico, idEspecialidad, fecha, hora, observaciones);

                Paciente paciente = pacNeg.ObtenerPorIdUsuario(usuario.Id);
                string emailDestino = paciente.Email;
                string nombrePaciente = paciente.Nombre;
                string medicoNombre = ddlMedico.SelectedItem.Text;

                EmailService emailService = new EmailService();
                emailService.EnviarConfirmacionTurno(emailDestino, nombrePaciente, fecha, medicoNombre);

                divExito.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "redir",
                    "setTimeout(function(){ window.location = 'MenuPaciente.aspx'; }, 2000);", true);


            }
            catch (SmtpException smtpEx)
            {
                lblError.Text = "Error al enviar el correo: " + smtpEx.Message;
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                MostrarError("Error: " + ex.Message);
            }
        }


        protected void calFecha_DayRender(object sender, DayRenderEventArgs e)
        {
            if (string.IsNullOrEmpty(ddlMedico.SelectedValue) || ddlMedico.SelectedValue == "0")
                return;

            int idMedico;


            if (!int.TryParse(ddlMedico.SelectedValue, out idMedico))
                return;

            string diaSemana = e.Day.Date.ToString("dddd", new CultureInfo("es-ES"));
            diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);

            TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();
            var turno = negocio.ObtenerHorario(idMedico, diaSemana);

            if (turno != null)
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        private void CargarDatosDelTurno(int idTurno)
        {
            TurnoNegocio negocio = new TurnoNegocio();
            Turno turno = negocio.BuscarPorId(idTurno);

            if (turno == null)
                return;

            
            ddlEspecialidad.SelectedValue = turno.Especialidad.Id.ToString();

            
            MedicoNegocio mNeg = new MedicoNegocio();
            var medicos = mNeg.ListarPorEspecialidad(turno.Especialidad.Id);

            ddlMedico.DataSource = medicos;
            ddlMedico.DataTextField = "NombreCompleto";
            ddlMedico.DataValueField = "Id";
            ddlMedico.DataBind();
            ddlMedico.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
            ddlMedico.SelectedValue = turno.Medico.Id.ToString();

            
            calFecha.SelectedDate = turno.Fecha;
            calFecha.VisibleDate = turno.Fecha;

            
            ddlHorario.Items.Clear();

            if (ddlMedico.SelectedValue != "0")
            {
                int idMedico = turno.Medico.Id;
                DateTime fecha = turno.Fecha;

                string diaSemana = fecha.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));
                diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);

                TurnoTrabajoNegocio trabNeg = new TurnoTrabajoNegocio();
                var horarioTrabajo = trabNeg.ObtenerHorario(idMedico, diaSemana);

                if (horarioTrabajo != null)
                {
                    List<TimeSpan> ocupados = negocio.ObtenerHorariosOcupados(idMedico, fecha);

                    TimeSpan inicio = horarioTrabajo.HoraInicio;
                    TimeSpan fin = horarioTrabajo.HoraFin;

                    for (TimeSpan h = inicio; h < fin; h = h.Add(TimeSpan.FromMinutes(30)))
                    {
                        if (!ocupados.Contains(h) || h == turno.Hora)
                            ddlHorario.Items.Add(h.ToString(@"hh\:mm"));
                    }

                    ddlHorario.SelectedValue = turno.Hora.ToString(@"hh\:mm");
                }
            }

            
            

           
            txtObservaciones.Text = turno.Observaciones;
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            lblError.Visible = true;
        }
        private void LimpiarErrores()
        {
            lblErrorEspecialidad.Text = "";
            lblErrorEspecialidad.Visible = false;

            lblErrorMedico.Text = "";
            lblErrorMedico.Visible = false;

            lblErrorFecha.Text = "";
            lblErrorFecha.Visible = false;

            lblErrorHorario.Text = "";
            lblErrorHorario.Visible = false;

           
        }

    }

}