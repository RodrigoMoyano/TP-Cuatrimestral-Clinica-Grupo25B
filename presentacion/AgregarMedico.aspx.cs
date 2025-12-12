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

                // inicializa lista temporal de turnos
                Session["TurnosTemp"] = new List<TurnoTrabajo>();
            }
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

        
        //  GUARDA USUARIO
    

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
               
                btnGuardarUsuario.Enabled = false;
                btnCancelarUsuario.Enabled = false;

            
                txtUsuario.Enabled = false;
                txtClave.Enabled = false;


            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
                throw;
            }
        }

        
       
        

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

                // Obtiene lista temporal
                List<TurnoTrabajo> lista = Session["TurnosTemp"] as List<TurnoTrabajo>;

                DayOfWeek diaEnum = (DayOfWeek)int.Parse(ddlDiaSemana.SelectedValue);
                string diaTexto = new System.Globalization.CultureInfo("es-ES")
                    .DateTimeFormat.GetDayName(diaEnum);
                // Crea turno nuevo
                TurnoTrabajo nuevo = new TurnoTrabajo
                {
                    DiaSemana = diaEnum,
                    DiaSemanaTexto = diaTexto,
                    HoraInicio = inicio,
                    HoraFin = fin
                };

                // Validacion para evitar duplicados
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

                // Actualiza grilla
                gvTurnos.DataSource = lista;
                gvTurnos.DataBind();

                ddlDiaSemana.SelectedItem.Enabled = false;
                Page.SetFocus(gvTurnos);
            }
            catch (Exception ex)
            {
                vsTurnos.HeaderText = $"Error al agregar turno: {ex.Message}";
            }
        }
        protected void gvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                List<TurnoTrabajo> lista = Session["TurnosTemp"] as List<TurnoTrabajo>;

                if (lista == null || index < 0 || index >= lista.Count)
                    return;

                TurnoTrabajo eliminado = lista[index];

                // Elimina turno
                lista.RemoveAt(index);

                // Actualizar grilla
                gvTurnos.DataSource = lista;
                gvTurnos.DataBind();

                // Rehabilitar el día eliminado
                var item = ddlDiaSemana.Items
                    .Cast<ListItem>()
                    .FirstOrDefault(i =>
                        i.Text.Equals(eliminado.DiaSemanaTexto, StringComparison.OrdinalIgnoreCase));

                if (item != null)
                    item.Enabled = true;
            }
        }

       
        //  GUARDA MEDICO DEFINITIVO
        

        protected void btnGuardarMedico_Click(object sender, EventArgs e)
        {
            Page.Validate("Medico");
            if (!Page.IsValid) return;


            // Limpiar mensajes previos
            lblErrorEmail.Text = "";
            lblErrorMatricula.Text = "";
            vsMedico.HeaderText = "";

            string telefono = txtTelefono.Text.Trim();

            if (!telefono.All(char.IsDigit) || telefono.Length > 12)
            {
                
                vsMedico.HeaderText = "El teléfono debe contener solo números y un máximo de 12 dígitos.";
                return;
            }

            //  Validación: al menos un turno laboral
            
            List<TurnoTrabajo> turnosTemp = Session["TurnosTemp"] as List<TurnoTrabajo>;
            if (turnosTemp == null || turnosTemp.Count == 0)
            {
                CustomValidator cv = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "❌ Debe agregar al menos un turno laboral antes de guardar.",
                    ValidationGroup = "Medico"
                };

                Page.Validators.Add(cv);
                return;
            }


            // Validación: al menos una especialidad seleccionada

            bool tieneEspecialidad = chkEspecialidades.Items.Cast<ListItem>().Any(i => i.Selected);
            if (!tieneEspecialidad)
            {
                CustomValidator cv = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "❌ Debe seleccionar al menos una especialidad antes de guardar.",
                    ValidationGroup = "Medico"
                };

                Page.Validators.Add(cv);
                return;
            }

            try
            {
                MedicoNegocio medicoNegocio = new MedicoNegocio();

                // Validación: email duplicado
                if (medicoNegocio.ExisteEmail(txtEmail.Text.Trim()))
                {
                    lblErrorEmail.Text = "❌ El email ingresado ya está registrado.";
                    return;
                }

                // Validación: matrícula duplicada
                if (medicoNegocio.ExisteMatricula(txtMatricula.Text.Trim()))
                {
                    lblErrorMatricula.Text = "❌ La matrícula ingresada ya está registrada.";
                    return;
                }

                // Crear objeto Médico
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

                //  Guardar médico en BD
                int idMedico = medicoNegocio.Agregar(medico);

                // Limpieza de sesión
                Session.Remove("TurnosTemp");

                // Redirección
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
            // No se crea usuario y se vuelve a gestion medicos
            

            Response.Redirect("GestionMedicos.aspx");
        }

        protected void btnCancelarMedico_Click(object sender, EventArgs e)
        {
            try
            {
                // 1 Si se había creado un usuario en la primera etapa
                if (Session["idUsuario"] != null)
                {
                    int idUsuario = Convert.ToInt32(Session["idUsuario"]);

                    // 2️⃣ Eliminar el usuario de la base de datos
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                    usuarioNegocio.Eliminar(idUsuario);
                }

                // Limpiar datos temporales de sesión
                Session.Remove("idUsuario");
                Session.Remove("TurnosTemp");

                // Volver a Gestión de Médicos
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