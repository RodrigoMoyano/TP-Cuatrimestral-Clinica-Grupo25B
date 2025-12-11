using System;
using System.Linq;
using System.Web.UI;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class DetalleMedico : PaginaMedico
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Validación de sesión
                if (Session["IdMedicoDetalle"] == null)
                {
                    Response.Redirect("GestionMedicos.aspx");
                    return;
                }

                int idMedico = (int)Session["IdMedicoDetalle"];

                // Buscar datos del médico
                MedicoNegocio negocio = new MedicoNegocio();
                Dominio.Medico medico = negocio.BuscarPorId(idMedico);

                if (medico == null)
                {
                    Session.Remove("IdMedicoDetalle");
                    Response.Redirect("GestionMedicos.aspx");
                    return;
                }


                lblId.Text = medico.Id.ToString();
                lblNombre.Text = medico.Nombre;
                lblApellido.Text = medico.Apellido;
                lblMatricula.Text = medico.Matricula;
                lblEmail.Text = medico.Email;
                lblTelefono.Text = medico.Telefono;


                if (medico.Especialidad != null && medico.Especialidad.Any())
                    lblEspecialidad.Text = string.Join(", ", medico.Especialidad.Select(esp => esp.Descripcion));
                else
                    lblEspecialidad.Text = "Sin especialidades registradas";


                TurnoTrabajoNegocio turnoNegocio = new TurnoTrabajoNegocio();
                var turnos = turnoNegocio.ListarPorMedico(idMedico);

                if (turnos != null && turnos.Count > 0)
                {
                    lblDisponibilidad.Text = string.Join("<br/>",
                        turnos.Select(t =>
                            $"{t.DiaSemanaTexto} {t.HoraInicio:hh\\:mm} - {t.HoraFin:hh\\:mm}"
                        )
                    );
                }
                else
                {
                    lblDisponibilidad.Text = "Sin horarios cargados";
                }
            }
        }


        private string TraducirDia(DayOfWeek dia)
        {
            switch (dia)
            {
                case DayOfWeek.Monday: return "Lunes";
                case DayOfWeek.Tuesday: return "Martes";
                case DayOfWeek.Wednesday: return "Miércoles";
                case DayOfWeek.Thursday: return "Jueves";
                case DayOfWeek.Friday: return "Viernes";
                case DayOfWeek.Saturday: return "Sábado";
                case DayOfWeek.Sunday: return "Domingo";
                default: return dia.ToString();
            }
        }


        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Session.Remove("IdMedicoDetalle");
            Response.Redirect("GestionMedicos.aspx");
        }
    }
}