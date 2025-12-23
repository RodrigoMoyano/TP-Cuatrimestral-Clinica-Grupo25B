using System;
using Dominio;
using Negocio;
using presentacion;

namespace Presentacion
{
    public partial class ObservacionesTurno : PaginaMedico
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Validar existencia del ID
                if (Request.QueryString["id"] == null)
                {
                    Response.Redirect("PanelMedico.aspx");
                    return;
                }

                int idTurno = Convert.ToInt32(Request.QueryString["id"]);

                // Obtener el turno
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                Turno turno = turnoNegocio.BuscarPorId(idTurno);

                if (turno == null)
                {
                    lblObservaciones.Text = "No se encontró el turno.";
                    return;
                }

                // Validación de seguridad → el médico solo ve sus turnos
                Usuario usuario = (Usuario)Session["Usuario"];

                if (usuario != null && usuario.Rol.Id == 3) // Médico
                {
                    int idMedicoSesion = (int)Session["IdMedico"];

                    if (idMedicoSesion != turno.Medico.Id)
                    {
                        Response.Redirect("PanelMedico.aspx");
                        return;
                    }
                }

                // Guardo el turno para usarlo luego en validaciones
                Session["TurnoActual"] = turno;

                //  Mostrar datos
                lblFecha.Text = "Fecha: " + turno.Fecha.ToString("dd/MM/yyyy");

                
                try
                {
                    string valorHora = turno.Hora.ToString();

                    if (!string.IsNullOrWhiteSpace(valorHora) &&
                        TimeSpan.TryParse(valorHora, out TimeSpan ts))
                    {
                        lblHora.Text = "Hora: " + ts.ToString(@"hh\:mm");
                    }
                    else
                    {
                        lblHora.Text = "Hora: -";
                    }
                }
                catch
                {
                    lblHora.Text = "Hora: -";
                }


                lblPaciente.Text = "Paciente: " +
                                   turno.Paciente.Nombre + " " + turno.Paciente.Apellido;

                lblEspecialidad.Text = "Especialidad: " +
                                       turno.Especialidad.Descripcion;

                lblEstado.Text = "Estado: " +
                                 turno.Estado.Descripcion;

                lblObservaciones.Text = "Observaciones: " + turno.Observaciones;

                // 5) Configurar qué botones puede ver el usuario
                ConfigurarAccionesPorRol();

                // 6) Validaciones según estado del turno
                AplicarValidacionesDeEstado(turno);
            }
        }


      
        private void AplicarValidacionesDeEstado(Turno turno)
        {
          
            if (turno.Estado.Id == 5) // Estado = Cerrado
                btnCerrar.Enabled = false;

            // No permitir "No asistió" si ya tiene ese estado
            if (turno.Estado.Id == 4)
                btnNoAsistio.Enabled = false;

            // No permitir modificar estados si el turno es anterior a hoy
            if (turno.Fecha.Date < DateTime.Today)
            {
                btnCerrar.Enabled = false;
                btnNoAsistio.Enabled = false;
                btnReprogramar.Enabled = false;
                btnCancelar.Enabled = false;
            }
        }


        
        private void ConfigurarAccionesPorRol()
        {
            Usuario usuario = (Usuario)Session["Usuario"];

            if (usuario == null)
            {
                btnReprogramar.Visible =
                btnCancelar.Visible =
                btnCerrar.Visible =
                btnNoAsistio.Visible = false;
                return;
            }

            int rol = usuario.Rol.Id; // 1=Admin, 2=Paciente, 3=Médico

            if (rol == 1) // ADMINISTRADOR
            {
                btnReprogramar.Visible = true;
                btnCancelar.Visible = true;
                btnCerrar.Visible = true;
                btnNoAsistio.Visible = true;
            }
            else if (rol == 3) // MÉDICO
            {
                btnReprogramar.Visible = false;
                btnCancelar.Visible = false;

                btnCerrar.Visible = true;
                btnNoAsistio.Visible = true;
            }
            else // PACIENTE
            {
                btnReprogramar.Visible =
                btnCancelar.Visible =
                btnCerrar.Visible =
                btnNoAsistio.Visible = false;
            }
        }


    
        protected void btnEditarObs_Click(object sender, EventArgs e)
        {
            txtObservaciones.Visible = true;
            btnGuardarObs.Visible = true;
            btnCancelarEdicion.Visible = true;

            lblObservaciones.Visible = false;
            btnEditarObs.Visible = false;

            txtObservaciones.Text =
                lblObservaciones.Text.Replace("Observaciones: ", "");
        }

        protected void btnGuardarObs_Click(object sender, EventArgs e)
        {
            int idTurno = Convert.ToInt32(Request.QueryString["id"]);
            new TurnoNegocio().ActualizarObservaciones(idTurno, txtObservaciones.Text);

            lblObservaciones.Text = "Observaciones: " + txtObservaciones.Text;

            txtObservaciones.Visible = false;
            btnGuardarObs.Visible = false;
            btnCancelarEdicion.Visible = false;

            lblObservaciones.Visible = true;
            btnEditarObs.Visible = true;
        }

        protected void btnCancelarEdicion_Click(object sender, EventArgs e)
        {
            txtObservaciones.Visible = false;
            btnGuardarObs.Visible = false;
            btnCancelarEdicion.Visible = false;

            lblObservaciones.Visible = true;
            btnEditarObs.Visible = true;
        }


       
        protected void btnReprogramar_Click(object sender, EventArgs e)
        {
            new TurnoNegocio().CambiarEstado(
                Convert.ToInt32(Request.QueryString["id"]), 2);
            Response.Redirect("PanelMedico.aspx");
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            var turno = (Turno)Session["TurnoActual"];

            if (turno.Estado.Id == 5) return; // Ya está cerrado

            new TurnoNegocio().CambiarEstado(turno.Id, 5);
            Response.Redirect("PanelMedico.aspx");
        }

        protected void btnNoAsistio_Click(object sender, EventArgs e)
        {
            var turno = (Turno)Session["TurnoActual"];

            if (turno.Estado.Id == 4) return; // Ya está marcado

            new TurnoNegocio().CambiarEstado(turno.Id, 4);
            Response.Redirect("PanelMedico.aspx");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            new TurnoNegocio().CambiarEstado(
                Convert.ToInt32(Request.QueryString["id"]), 3);
            Response.Redirect("PanelMedico.aspx");
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("PanelMedico.aspx");
        }
    }
}