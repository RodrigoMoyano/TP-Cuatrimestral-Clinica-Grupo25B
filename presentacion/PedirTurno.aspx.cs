using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class PedirTurno : PaginaPaciente
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspecialidades();
                CargarCoberturas();
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
        private void CargarCoberturas()
        {
           
            CoberturaNegocio negocio = new CoberturaNegocio();

            var lista = negocio.Listar()
                               .Select(x => x.Tipo)
                               .Distinct()
                               .ToList();

            ddlCobertura.DataSource = lista;
            ddlCobertura.DataBind();

            ddlCobertura.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }
        protected void ddlCobertura_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlObraSocial.Items.Clear();

            
            if (ddlCobertura.SelectedValue == "Particular")
            {
                ddlObraSocial.Enabled = false;
                return;
            }

            
            CoberturaNegocio negocio = new CoberturaNegocio();
            var lista = negocio.Listar()
                               .Where(x => x.Tipo == "Obra Social")
                               .ToList();

            ddlObraSocial.Enabled = true;
            ddlObraSocial.DataSource = lista;
            ddlObraSocial.DataTextField = "NombreObraSocial";
            ddlObraSocial.DataValueField = "Id";
            ddlObraSocial.DataBind();
            ddlObraSocial.Items.Insert(0, new ListItem("-- Seleccione Obra Social --", ""));
        }
        protected void ddlObraSocial_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = int.Parse(ddlObraSocial.SelectedValue);

            CoberturaNegocio negocio = new CoberturaNegocio();
            var cobertura = negocio.Listar().First(x => x.Id == id);
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

            if (ddlCobertura.SelectedValue == "")
            {
                lblErrorCobertura.Text = "Debe seleccionar una cobertura.";
                lblErrorCobertura.Visible = true;
                hayError = true;
            }

            if (ddlCobertura.SelectedValue == "Obra Social" &&
                ddlObraSocial.SelectedValue == "")
            {
                lblErrorObraSocial.Text = "Debe seleccionar una obra social.";
                lblErrorObraSocial.Visible = true;
                hayError = true;
            }

            if (hayError) return;



            Usuario usuario = (Usuario)Session["Usuario"];

            
            PacienteNegocio pacNeg = new PacienteNegocio();
            int idPaciente = pacNeg.ObtenerIdPacientePorIdUsuario(usuario.Id);

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

            TurnoNegocio negocio = new TurnoNegocio();
            negocio.Agregar(idPaciente, idMedico, idEspecialidad, fecha, hora, observaciones);

            
            Response.Redirect("MenuPaciente.aspx");
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

            lblErrorCobertura.Text = "";
            lblErrorCobertura.Visible = false;

            lblErrorObraSocial.Text = "";
            lblErrorObraSocial.Visible = false;
        }

    }
    
}