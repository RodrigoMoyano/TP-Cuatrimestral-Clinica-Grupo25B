using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class AgregarMedico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarRoles();
                CargarEspecialidades();
                CargarHorarios();

                // Inicializar lista temporal de turnos
                Session["TurnosTemp"] = new List<TurnoTrabajo>();
            }
        }

        // ============================================================
        //   CARGA INICIAL DE CONTROLES
        // ============================================================

        private void CargarRoles()
        {
            RolNegocio rolNegocio = new RolNegocio();

            ddlRol.DataSource = rolNegocio.Listar();
            ddlRol.DataTextField = "Descripcion";
            ddlRol.DataValueField = "Id";
            ddlRol.DataBind();
            ddlRol.Items.Insert(0, new ListItem("-- Seleccione un rol --", ""));
        }

        private void CargarEspecialidades()
        {
            EspecialidadNegocio espNegocio = new EspecialidadNegocio();

            chkEspecialidades.DataSource = espNegocio.Listar();
            chkEspecialidades.DataTextField = "Descripcion";
            chkEspecialidades.DataValueField = "Id";
            chkEspecialidades.DataBind();
        }

        private void CargarHorarios()
        {
            TimeSpan inicio = TimeSpan.FromHours(8);
            TimeSpan fin = TimeSpan.FromHours(20);
            TimeSpan hora = inicio;

            while (hora <= fin)
            {
                string t = hora.ToString(@"hh\:mm");

                ddlHoraInicio.Items.Add(new ListItem(t, t));
                ddlHoraFin.Items.Add(new ListItem(t, t));

                hora = hora.Add(TimeSpan.FromMinutes(30));
            }
        }

        // ============================================================
        //       GUARDAR USUARIO
        // ============================================================

        protected void btnGuardarUsuario_Click(object sender, EventArgs e)
        {
            Page.Validate("Usuario");
            if (!Page.IsValid) return; // No habilita panel si hay errores

            try
            {
                Usuario usuario = new Usuario
                {
                    NombreUsuario = txtUsuario.Text,
                    Clave = txtClave.Text,
                    Activo = true,
                    Rol = new Rol { Id = int.Parse(ddlRol.SelectedValue) }
                };

                UsuarioNegocio negocio = new UsuarioNegocio();
                int idUsuario = negocio.AgregarYObtenerId(usuario);

                Session["idUsuario"] = idUsuario;

                pnlMedico.Visible = true;
            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
                throw;
            }
        }

        // ============================================================
        //     BOTÓN: AGREGAR TURNO 
        // ============================================================

        protected void btnAgregarTurno_Click(object sender, EventArgs e)
        {
            Page.Validate("Turno");
            if (!Page.IsValid) return;

            try
            {
                if (string.IsNullOrEmpty(ddlHoraInicio.SelectedValue) ||
                    string.IsNullOrEmpty(ddlHoraFin.SelectedValue))
                {
                    vsTurnos.HeaderText = "Debe seleccionar hora de inicio y hora de fin";
                    return;
                }

                TimeSpan inicio = TimeSpan.Parse(ddlHoraInicio.SelectedValue);
                TimeSpan fin = TimeSpan.Parse(ddlHoraFin.SelectedValue);

                if (inicio >= fin)
                {
                    vsTurnos.HeaderText = "La hora de inicio debe ser menor que la hora de fin";
                    return;
                }

                // Obtener lista temporal
                List<TurnoTrabajo> lista = Session["TurnosTemp"] as List<TurnoTrabajo>;

                DayOfWeek diaEnum = (DayOfWeek)int.Parse(ddlDiaSemana.SelectedValue);
                string diaTexto = new System.Globalization.CultureInfo("es-ES")
                    .DateTimeFormat.GetDayName(diaEnum);
                // Crear turno nuevo
                TurnoTrabajo nuevo = new TurnoTrabajo
                {
                    DiaSemana = diaEnum,
                    DiaSemanaTexto = diaTexto,
                    HoraInicio = inicio,
                    HoraFin = fin
                };

                // Validación para evitar duplicados
                bool yaExiste = lista.Any(t =>
                    t.DiaSemana == nuevo.DiaSemana &&
                    t.HoraInicio == nuevo.HoraInicio &&
                    t.HoraFin == nuevo.HoraFin);

                if (yaExiste)
                {
                    vsTurnos.HeaderText = "Ese turno ya está cargado";
                    return;
                }

                lista.Add(nuevo);

                // Actualizar grilla
                gvTurnos.DataSource = lista;
                gvTurnos.DataBind();

                ddlDiaSemana.SelectedItem.Enabled = false;
            }
            catch (Exception ex)
            {
                vsTurnos.HeaderText = $"Error al agregar turno: {ex.Message}";
            }
        }

        // ============================================================
        //     GUARDAR MÉDICO DEFINITIVO
        // ============================================================

        protected void btnGuardarMedico_Click(object sender, EventArgs e)
        {
            Page.Validate("Medico");
            if (!Page.IsValid) return; // No guarda si hay errores

            try
            {
                List<TurnoTrabajo> turnosTemp = Session["TurnosTemp"] as List<TurnoTrabajo>;

                Medico medico = new Medico
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Matricula = txtMatricula.Text,
                    Telefono = txtTelefono.Text,
                    Email = txtEmail.Text,
                    Especialidad = new List<Especialidad>(),
                    TurnosTrabajo = turnosTemp,
                    Usuario = new Usuario
                    {
                        Id = Convert.ToInt32(Session["idUsuario"])
                    },
                    IdUsuario = Convert.ToInt32(Session["idUsuario"])
                };

                foreach (ListItem item in chkEspecialidades.Items)
                {
                    if (item.Selected)
                    {
                        medico.Especialidad.Add(new Especialidad
                        {
                            Id = int.Parse(item.Value)
                        });
                    }
                }

                // Guardar médico
                MedicoNegocio negocio = new MedicoNegocio();
                int idMedico = negocio.Agregar(medico);

                // Limpiar sesión
                Session.Remove("TurnosTemp");

                Response.Redirect("GestionMedicos.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
                throw;
            }
        }
    }
}