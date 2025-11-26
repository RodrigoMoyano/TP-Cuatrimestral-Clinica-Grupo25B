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
    public partial class PedirTurno : System.Web.UI.Page
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
            /*CoberturaNegocio negocio = new CoberturaNegocio();
            ddlCobertura.DataSource = negocio.Listar();
            ddlCobertura.DataTextField = "Tipo";
            ddlCobertura.DataValueField = "Id";
            ddlCobertura.DataBind();*/
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

            // Si es Particular, no mostrar nada
            if (ddlCobertura.SelectedValue == "Particular")
            {
                ddlObraSocial.Enabled = false;
                return;
            }

            // Si es Obra Social, cargar OSDE, Swiss, etc
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
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "alert",
                    "alert('No se puede seleccionar una fecha pasada.');",
                    true);

                calFecha.SelectedDates.Clear();
                return;
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
            if (ddlEspecialidad.SelectedValue == "0" ||
                ddlMedico.SelectedValue == "0" ||
                ddlHorario.SelectedValue == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Debe completar todos los campos');", true);
                return;
            }
            if (calFecha.SelectedDate == DateTime.MinValue)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                "alert('Debe seleccionar una fecha.');", true);
                return;
            }

            
            if (ddlCobertura.SelectedValue == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                "alert('Debe seleccionar una cobertura (Particular u Obra Social');",true);
                return;
            }

            
            if (ddlCobertura.SelectedValue == "Obra Social" &&
                ddlObraSocial.SelectedValue == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                "alert('Debe seleccionar una obra social.');", true);
                return;
            }


            Usuario usuario = (Usuario)Session["usuario"];

            //  Obtener el idPaciente real desde la base de datos
            PacienteNegocio pacNeg = new PacienteNegocio();
            int idPaciente = pacNeg.ObtenerIdPacientePorIdUsuario(usuario.Id);

            int idMedico = int.Parse(ddlMedico.SelectedValue);
            int idEspecialidad = int.Parse(ddlEspecialidad.SelectedValue);
            DateTime fecha = calFecha.SelectedDate;
            TimeSpan hora = TimeSpan.Parse(ddlHorario.SelectedValue);

            string observaciones = txtObservaciones.Text.Trim();

            if (fecha.Date < DateTime.Now.Date)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('No se puede sacar un turno para una fecha pasada.');", true);
                return;
            }

            
            if (fecha.Date == DateTime.Now.Date && hora <= DateTime.Now.TimeOfDay)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('No se puede sacar un turno en una hora que ya pasó.');", true);
                return;
            }

            TurnoNegocio negocio = new TurnoNegocio();
            negocio.Agregar(idPaciente, idMedico, idEspecialidad, fecha, hora, observaciones);

            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                "alert('Turno reservado con éxito'); window.location='MenuPaciente.aspx';", true);
        }
        //PARA QUE SE NOTEN LOS DIAS EN QUE TRABAJA CADA MEDICO
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
        
    }
    
}