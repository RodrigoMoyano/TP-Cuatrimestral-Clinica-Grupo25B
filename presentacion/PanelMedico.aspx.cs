using Dominio;
using Negocio;
using presentacion;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class PanelMedico : PaginaMedico
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // ✔ Validación segura de sesión
            if (Session["IdMedico"] == null ||
                string.IsNullOrWhiteSpace(Session["IdMedico"].ToString()))
            {
                Response.Redirect("LogIn.aspx", false);
                return;
            }

            int idMedico = (int)Session["IdMedico"]; // ✔ ahora es 100% seguro

            if (!IsPostBack)
            {
                // ✔ Datos del médico
                MedicoNegocio medicoNegocio = new MedicoNegocio();
                Medico medico = medicoNegocio.BuscarPorId(idMedico);

                lblNombreMedico.Text = medico.Nombre + " " + medico.Apellido;
                lblEspecialidades.Text = string.Join(", ",
                    medico.Especialidad.Select(esp => esp.Descripcion));

                // ✔ Cargar turnos
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                gvTurnos.DataSource = turnoNegocio.ObtenerPorMedico(idMedico);
                gvTurnos.DataBind();
            }
        }

        protected void gvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerObservaciones")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int idTurno = Convert.ToInt32(gvTurnos.DataKeys[index].Value);

                Response.Redirect("ObservacionesTurno.aspx?id=" + idTurno);
            }
        }

        protected string FormatearHora(object horaObj)
        {
            if (horaObj == null)
                return "-";

            // Si ya es TimeSpan
            if (horaObj is TimeSpan ts)
                return ts.ToString(@"hh\:mm");

            // Si viene como string “08:00”, “08:00:00”, etc.
            if (TimeSpan.TryParse(horaObj.ToString(), out ts))
                return ts.ToString(@"hh\:mm");

            // Cualquier otro caso
            return horaObj.ToString();
        }
    }
}


