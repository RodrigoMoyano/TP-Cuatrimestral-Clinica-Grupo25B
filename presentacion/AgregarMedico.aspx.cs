using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class AgregarMedico : PaginaAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CargarEspecialidades();
                CargarHorarios();

                // Inicializar lista temporal de turnos
                Session["TurnosTemp"] = new List<TurnoTrabajo>();
            }
        }

        // ============================================================
        //   CARGA INICIAL DE CONTROLES
        // ============================================================



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
            lblErrorUsuario.Text = ""; // limpiar mensaje previo

            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();

                if (negocio.ExisteUsuario(txtUsuario.Text.Trim()))
                {
                    lblErrorUsuario.Text = "❌ El nombre de usuario ya existe. Elija otro.";
                    return;
                }

                // Validar si el usuario ya existe



                Usuario usuario = new Usuario
                {
                    NombreUsuario = txtUsuario.Text,
                    Clave = txtClave.Text,
                    Activo = true,
                    Rol = new Rol { Id = 3 }
                };

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
            if (!Page.IsValid) return;

            // Limpiar mensajes previos
            lblErrorEmail.Text = "";
            lblErrorMatricula.Text = "";
            vsMedico.HeaderText = "";

            // 1️⃣ Validación: al menos un turno laboral
            List<TurnoTrabajo> turnosTemp = Session["TurnosTemp"] as List<TurnoTrabajo>;
            if (turnosTemp == null || turnosTemp.Count == 0)
            {
                vsMedico.HeaderText = "Debe agregar al menos un turno laboral antes de guardar";
                return;
            }

            // 2️⃣ Validación: al menos una especialidad seleccionada
            bool tieneEspecialidad = chkEspecialidades.Items.Cast<ListItem>().Any(i => i.Selected);
            if (!tieneEspecialidad)
            {
                vsMedico.HeaderText = "Debe seleccionar al menos una especialidad antes de guardar";
                return;
            }

            try
            {
                MedicoNegocio medicoNegocio = new MedicoNegocio();

                // 3️⃣ Validación: email duplicado
                if (medicoNegocio.ExisteEmail(txtEmail.Text.Trim()))
                {
                    lblErrorEmail.Text = "❌ El email ingresado ya está registrado.";
                    return;
                }

                // 4️⃣ Validación: matrícula duplicada
                if (medicoNegocio.ExisteMatricula(txtMatricula.Text.Trim()))
                {
                    lblErrorMatricula.Text = "❌ La matrícula ingresada ya está registrada.";
                    return;
                }

                // 5️⃣ Crear objeto Médico
                Medico medico = new Medico
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Matricula = txtMatricula.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Especialidad = new List<Especialidad>(),
                    TurnosTrabajo = turnosTemp,
                    Usuario = new Usuario
                    {
                        Id = Convert.ToInt32(Session["idUsuario"])
                    },
                    IdUsuario = Convert.ToInt32(Session["idUsuario"])
                };

                // Agregar especialidades seleccionadas
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

                // 6️⃣ Guardar médico en BD
                int idMedico = medicoNegocio.Agregar(medico);

                // Limpieza de sesión
                Session.Remove("TurnosTemp");

                // 7️⃣ Redirección
                Response.Redirect("GestionMedicos.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
                throw;
            }
        }

        protected void btnCancelarUsuario_Click(object sender, EventArgs e)
        {
            // No se ha creado ningún usuario todavía
            // Así que simplemente volvemos a la gestión de médicos

            Response.Redirect("GestionMedicos.aspx");
        }

        protected void btnCancelarMedico_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Recupero el usuario que se creó en la etapa anterior
                if (Session["idUsuario"] != null)
                {
                    int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                    // 2️⃣ Elimino el usuario de la base de datos
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                    usuarioNegocio.Eliminar(idUsuario);
                }

                // 3️⃣ Limpio datos temporales
                Session.Remove("idUsuario");
                Session.Remove("TurnosTemp");

                // 4️⃣ Vuelvo a Gestión de Médicos
                Response.Redirect("GestionMedicos.aspx");
            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
                throw;
            }
        }
    }
}