using Dominio;
using Negocio;
using presentacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class PanelMedico : PaginaMedico
    {
        // 🔹 Guardar lista original en sesión
        private List<Turno> listaTurnosOriginal
        {
            get { return Session["TurnosOriginal"] as List<Turno>; }
            set { Session["TurnosOriginal"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // ✔ Validación segura
            if (Session["IdMedico"] == null ||
                string.IsNullOrWhiteSpace(Session["IdMedico"].ToString()))
            {
                Response.Redirect("LogIn.aspx", false);
                return;
            }

            int idMedico = (int)Session["IdMedico"];

            if (!IsPostBack)
            {
                // ✔ Datos del médico
                MedicoNegocio medicoNegocio = new MedicoNegocio();
                Medico medico = medicoNegocio.BuscarPorId(idMedico);

                lblNombreMedico.Text = medico.Nombre + " " + medico.Apellido;

                lblEspecialidades.Text =
                    string.Join(", ", medico.Especialidad.Select(esp => esp.Descripcion));

                // ✔ Cargar turnos del médico
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                listaTurnosOriginal = turnoNegocio.ObtenerPorMedico(idMedico);

                gvTurnos.DataSource = listaTurnosOriginal;
                gvTurnos.DataBind();
            }
        }

        // ================================================================
        // 🔵 MÉTODO CENTRAL DE FILTRADO
        // ================================================================
        private void AplicarFiltros()
        {
            if (listaTurnosOriginal == null) return;

            IEnumerable<Turno> lista = listaTurnosOriginal;

            // 🔹 Filtro por paciente
            string pacienteFiltro = txtBuscarPaciente.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(pacienteFiltro))
            {
                lista = lista.Where(t =>
                    (t.Paciente.Nombre + " " + t.Paciente.Apellido)
                        .ToLower()
                        .Contains(pacienteFiltro));
            }

            // 🔹 Filtro por Estado
            if (ddlEstado.SelectedValue != "0")
            {
                lista = lista.Where(t =>
                    t.Estado.Descripcion == ddlEstado.SelectedValue);
            }

            // 🔹 Ordenar por fecha
            if (ddlOrdenFecha.SelectedValue == "asc")
                lista = lista.OrderBy(t => t.Fecha);
            else
                lista = lista.OrderByDescending(t => t.Fecha);

            // 🔹 Cargar lista filtrada
            gvTurnos.DataSource = lista.ToList();
            gvTurnos.DataBind();
        }

        // ================================================================
        // 🔵 EVENTOS DE FILTRADO
        // ================================================================
        protected void txtBuscarPaciente_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void ddlOrdenFecha_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        // ================================================================
        // 🔵 BOTÓN DE VER OBSERVACIONES
        // ================================================================
        protected void gvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerObservaciones")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int idTurno = Convert.ToInt32(gvTurnos.DataKeys[index].Value);

                Response.Redirect("ObservacionesTurno.aspx?id=" + idTurno);
            }
        }

        // ================================================================
        // 🔵 FUNCIÓN SEGURA PARA FORMATEAR HORA
        // ================================================================
        protected string FormatearHora(object horaObj)
        {
            string hs = horaObj?.ToString();

            if (!string.IsNullOrWhiteSpace(hs)
                && TimeSpan.TryParse(hs, out TimeSpan ts))
                return ts.ToString(@"hh\:mm");

            return "-";
        }
    }
}