using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class EditarMedico : PaginaAdmin
    {
        private int idMedico;

        private List<TurnoTrabajo> turnosTemp
        {
            get { return Session["TurnosTemp"] as List<TurnoTrabajo>; }
            set { Session["TurnosTemp"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdMedicoEditar"] == null)
                Response.Redirect("GestionMedicos.aspx");

            idMedico = (int)Session["IdMedicoEditar"];

            if (!IsPostBack)
            {
                CargarHorarios();
                CargarEspecialidades();
                CargarMedico();
            }
        }

        private void CargarHorarios()
        {
            TimeSpan h = TimeSpan.FromHours(8);
            TimeSpan fin = TimeSpan.FromHours(20);

            while (h <= fin)
            {
                string txt = h.ToString(@"hh\:mm");
                ddlHoraInicio.Items.Add(new ListItem(txt, txt));
                ddlHoraFin.Items.Add(new ListItem(txt, txt));
                h = h.Add(TimeSpan.FromMinutes(30));
            }
        }

        private void CargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            chkEspecialidades.DataSource = negocio.Listar();
            chkEspecialidades.DataValueField = "Id";
            chkEspecialidades.DataTextField = "Descripcion";
            chkEspecialidades.DataBind();
        }

        private void CargarMedico()
        {
            MedicoNegocio negocio = new MedicoNegocio();
            Medico med = negocio.BuscarPorId(idMedico);

            if (med == null)
            {
                Response.Redirect("GestionMedicos.aspx");
                return;
            }

            txtNombre.Text = med.Nombre;
            txtApellido.Text = med.Apellido;
            txtMatricula.Text = med.Matricula;
            txtTelefono.Text = med.Telefono;
            txtEmail.Text = med.Email;

            foreach (var esp in med.Especialidad)
            {
                var item = chkEspecialidades.Items.FindByValue(esp.Id.ToString());
                if (item != null)
                    item.Selected = true;
            }

            turnosTemp = med.TurnosTrabajo ?? new List<TurnoTrabajo>();
            gvTurnos.DataSource = turnosTemp;
            gvTurnos.DataBind();

            // DESHABILITAR DÍAS YA ELEGIDOS — CORREGIDO
            foreach (var t in turnosTemp)
            {
                var item = ddlDiaSemana.Items.FindByText(t.DiaSemanaTexto);
                if (item != null)
                    item.Enabled = false;
            }
        }

        protected void btnAgregarTurno_Click(object sender, EventArgs e)
        {
            Page.Validate("Turno");
            if (!Page.IsValid) return;

            TimeSpan inicio = TimeSpan.Parse(ddlHoraInicio.SelectedValue);
            TimeSpan fin = TimeSpan.Parse(ddlHoraFin.SelectedValue);

            if (inicio >= fin)
            {
                CustomValidator cv = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "La hora de inicio debe ser menor que la hora de fin.",
                    ValidationGroup = "Turno"
                };
                Page.Validators.Add(cv);
                return;
            }

            TurnoTrabajo nuevo = new TurnoTrabajo
            {
                DiaSemana = (DayOfWeek)int.Parse(ddlDiaSemana.SelectedValue),
                DiaSemanaTexto = ddlDiaSemana.SelectedItem.Text,   // ✔ CORREGIDO
                HoraInicio = inicio,
                HoraFin = fin,
                IdMedico = idMedico
            };

            bool yaExiste = turnosTemp.Any(t =>
                t.DiaSemanaTexto == nuevo.DiaSemanaTexto &&
                t.HoraInicio == nuevo.HoraInicio &&
                t.HoraFin == nuevo.HoraFin
            );

            if (yaExiste)
            {
                CustomValidator cv = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "Ese turno ya está cargado.",
                    ValidationGroup = "Turno"
                };
                Page.Validators.Add(cv);
                return;
            }

            turnosTemp.Add(nuevo);
            gvTurnos.DataSource = turnosTemp;
            gvTurnos.DataBind();

            // DESHABILITAR DÍA SELECCIONADO — CORREGIDO
            ddlDiaSemana.Items.FindByText(nuevo.DiaSemanaTexto).Enabled = false;
        }

        protected void gvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                var eliminado = turnosTemp[index];
                turnosTemp.RemoveAt(index);

                gvTurnos.DataSource = turnosTemp;
                gvTurnos.DataBind();

                // REHABILITAR DÍA — CORREGIDO
                var item = ddlDiaSemana.Items.FindByText(eliminado.DiaSemanaTexto);
                if (item != null)
                    item.Enabled = true;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Medico med = new Medico
            {
                Id = idMedico,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Matricula = txtMatricula.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Especialidad = new List<Especialidad>(),
                TurnosTrabajo = turnosTemp
            };

            foreach (ListItem item in chkEspecialidades.Items)
            {
                if (item.Selected)
                    med.Especialidad.Add(new Especialidad { Id = int.Parse(item.Value) });
            }

            MedicoNegocio negocio = new MedicoNegocio();
            negocio.Modificar(med);

            TurnoTrabajoNegocio turnosNeg = new TurnoTrabajoNegocio();
            turnosNeg.EliminarPorMedico(idMedico);

            foreach (var t in turnosTemp)
                turnosNeg.Agregar(t);

            Response.Redirect("GestionMedicos.aspx");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionMedicos.aspx");
        }
    }
}