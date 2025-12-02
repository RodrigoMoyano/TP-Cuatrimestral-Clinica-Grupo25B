using Dominio;
using Negocio;
using System;
using System.Collections.Generic;

namespace presentacion
{
    public partial class AgregarMedico : System.Web.UI.Page
    {
        // Guardamos el IdUsuario recién creado en Session
        private int idUsuario
        {
            get { return (int)(Session["IdUsuarioNuevo"] ?? 0); }
            set { Session["IdUsuarioNuevo"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cargar roles
                RolNegocio rolNegocio = new RolNegocio();
                ddlRol.DataSource = rolNegocio.Listar();
                ddlRol.DataTextField = "Descripcion";
                ddlRol.DataValueField = "Id";
                ddlRol.DataBind();

                // Cargar especialidades
                EspecialidadNegocio espNegocio = new EspecialidadNegocio();
                ddlEspecialidad.DataSource = espNegocio.Listar();
                ddlEspecialidad.DataTextField = "Descripcion";
                ddlEspecialidad.DataValueField = "Id";
                ddlEspecialidad.DataBind();
            }
        }

        // Paso 1: Guardar Usuario
        protected void btnGuardarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = new Usuario
                {
                    NombreUsuario = txtUsuario.Text,
                    Clave = txtClave.Text,
                    Activo = true,
                    Rol = new Rol { Id = int.Parse(ddlRol.SelectedValue) }
                };

                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                idUsuario = usuarioNegocio.AgregarYObtenerId(usuario); // ✅ devuelve el Id generado

                // Mostrar panel de datos del médico
                pnlMedico.Visible = true;
            }
            catch (Exception ex)
            {
                Session.Add("error", ex);
                throw;
            }
        }
        // La propiedad Especialidad es una lista, cargamos un solo elemento
        // La propiedad Especialidad es una lista, cargamos un solo elemento
        // Paso 2: Guardar Médico vinculado al Usuario
        protected void btnGuardarMedico_Click(object sender, EventArgs e)
        {
            try
            {
                Dominio.Medico medico = new Dominio.Medico
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Matricula = txtMatricula.Text,
                    Telefono = txtTelefono.Text,
                    Email = txtEmail.Text,
                    // La propiedad Especialidad es una lista, cargamos un solo elemento
                    Especialidad = new List<Especialidad>
                    {
                        new Especialidad { Id = int.Parse(ddlEspecialidad.SelectedValue) }
                    },
                    Usuario = new Usuario { Id = idUsuario },
                    IdUsuario = idUsuario
                };

                MedicoNegocio medicoNegocio = new MedicoNegocio();
                medicoNegocio.Agregar(medico);

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