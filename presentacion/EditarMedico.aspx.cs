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

            // DESHABILITAR DIAS YA ELEGIDOS — CORREGIDO
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

            // DESHABILITAR DIA SELECCIONADO — CORREGIDO
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

                // REHABILITAR DIA — CORREGIDO
                var item = ddlDiaSemana.Items.FindByText(eliminado.DiaSemanaTexto);
                if (item != null)
                    item.Enabled = true;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
                return;
            string telefono = txtTelefono.Text.Trim();

            if (!telefono.All(char.IsDigit) || telefono.Length > 12)
            {
                CustomValidator cv = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "El teléfono debe contener solo números y un máximo de 12 dígitos."
                };
                Page.Validators.Add(cv);
                return;
            }

            // 🔹 Validación: campos obligatorios
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtMatricula.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                CustomValidator cv = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "Todos los campos del médico son obligatorios."
                };
                Page.Validators.Add(cv);
                return;
            }

            //  Validación: al menos una especialidad
            bool tieneEspecialidad = chkEspecialidades.Items.Cast<ListItem>()
                                        .Any(i => i.Selected);

            if (!tieneEspecialidad)
            {
                CustomValidator cv = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "Debe seleccionar al menos una especialidad."
                };
                Page.Validators.Add(cv);
                return;
            }

            MedicoNegocio negocio = new MedicoNegocio();

            //  Validación: email duplicado
            if (negocio.ExisteEmail(txtEmail.Text.Trim(), idMedico))
            {
                CustomValidator cv = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "El email ingresado ya pertenece a otro médico."
                };
                Page.Validators.Add(cv);
                return;
            }

            //  Validación: matrícula duplicada
            if (negocio.ExisteMatricula(txtMatricula.Text.Trim(), idMedico))
            {
                CustomValidator cv = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "La matrícula ingresada ya pertenece a otro médico."
                };
                Page.Validators.Add(cv);
                return;
            }

            //  Construcción del médico
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
                {
                    med.Especialidad.Add(
                        new Especialidad { Id = int.Parse(item.Value) });
                }
            }

            //  Guardar cambios
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