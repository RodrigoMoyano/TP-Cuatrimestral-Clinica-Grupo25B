using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class EditarPaciente : PaginaAdmin
    {

        PacienteNegocio pacienteNegocio = new PacienteNegocio();
        UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
        CoberturaNegocio coberturaNegocio = new CoberturaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCoberturas();
                

                if (Request.QueryString["id"] != null)
                {
                    int idPaciente = int.Parse(Request.QueryString["id"]);
                    CargarDatos(idPaciente);
                }
            }
        }
        private void CargarCoberturas()
        {

            CoberturaNegocio negocio = new CoberturaNegocio();

            var lista = negocio.Listar()
                               .Select(x => x.Tipo)
                               .Distinct()
                               .ToList();

            ddlCobertura.DataSource = lista;
            ddlCobertura.DataBind();

            ddlCobertura.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }
        protected void ddlCobertura_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlObraSocial.Items.Clear();


            if (ddlCobertura.SelectedValue == "Particular")
            {
                ddlObraSocial.Enabled = false;
                return;
            }


            CoberturaNegocio negocio = new CoberturaNegocio();
            var lista = negocio.Listar()
                               .Where(x => x.Tipo == "Obra Social")
                               .ToList();

            ddlObraSocial.Enabled = true;
            ddlObraSocial.DataSource = lista;
            ddlObraSocial.DataTextField = "NombreObraSocial";
            ddlObraSocial.DataValueField = "Id";
            ddlObraSocial.DataBind();
            ddlObraSocial.Items.Insert(0, new ListItem("-- Seleccione Obra Social --", ""));
        }
        protected void ddlObraSocial_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = int.Parse(ddlObraSocial.SelectedValue);

            CoberturaNegocio negocio = new CoberturaNegocio();
            var cobertura = negocio.Listar().First(x => x.Id == id);
        }
        private void CargarDatos(int idPaciente)
        {
            Paciente p = pacienteNegocio.ObtenerPorId(idPaciente);
            if (p == null) return;


            if (p.Usuario != null)
            {
                txtIdUsuario.Text = p.Usuario.Id.ToString();
                txtNombreUsuario.Text = p.Usuario.NombreUsuario;


                txtPassword.Attributes["value"] = p.Usuario.Clave ?? "";


                ddlActivoUsuario.SelectedValue = p.Usuario.Activo ? "true" : "false";
            }
            else
            {
                txtIdUsuario.Text = "0";
                txtNombreUsuario.Text = "";
                ddlActivoUsuario.SelectedValue = "true";
            }


            txtIdPaciente.Text = p.Id.ToString();
            txtNombre.Text = p.Nombre ?? "";
            txtApellido.Text = p.Apellido ?? "";
            txtDni.Text = p.Dni ?? "";
            txtEmail.Text = p.Email ?? "";
            txtTelefono.Text = p.Telefono ?? "";

            if (p.Cobertura != null && p.Cobertura.Id > 1)
            
            {
                ddlCobertura.SelectedValue = "Obra Social";

                
                ddlObraSocial.Enabled = true;
                ddlObraSocial.DataSource = coberturaNegocio.Listar().Where(x => x.Tipo == "Obra Social").ToList();
                ddlObraSocial.DataTextField = "NombreObraSocial";
                ddlObraSocial.DataValueField = "Id";
                ddlObraSocial.DataBind();

                
                ddlObraSocial.SelectedValue = p.Cobertura.Id.ToString();
            }
            else
            {
                ddlCobertura.SelectedValue = "Particular";
                ddlObraSocial.Items.Clear();
                ddlObraSocial.Enabled = false;
            }
        }


        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionPaciente.aspx");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                
                Usuario u = usuarioNegocio.ObtenerPorId(int.Parse(txtIdUsuario.Text));

                if (u == null)
                    throw new Exception("No se encontró el usuario asociado.");

               
                u.NombreUsuario = txtNombreUsuario.Text.Trim();
                u.Clave = txtPassword.Text.Trim();
                u.Activo = ddlActivoUsuario.SelectedValue == "true";

                usuarioNegocio.Modificar(u);

                
                Paciente p = new Paciente
                {
                    Id = int.Parse(txtIdPaciente.Text),
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Dni = txtDni.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Usuario = u
                };

                
                if (ddlCobertura.SelectedValue.Equals("Obra Social", StringComparison.OrdinalIgnoreCase)
                    && int.TryParse(ddlObraSocial.SelectedValue, out int idCob) && idCob > 0)
                {
                    p.Cobertura = new Cobertura { Id = idCob };
                }
                else
                {

                    p.Cobertura = new Cobertura { Id = 1 };
                }

                pacienteNegocio.Modificar(p); 

                lblMensaje.ForeColor = System.Drawing.Color.Green;
                lblMensaje.Text = "Datos guardados correctamente.";
            }
            catch (Exception ex)
            {
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                lblMensaje.Text = "Error al guardar: " + ex.Message;
            }
        }
    }
}