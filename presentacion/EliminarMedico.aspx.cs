using System;
using System.Linq;
using System.Web.UI;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class EliminarMedico : PaginaAdmin
    {
        private int idMedico;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["IdMedicoEliminar"] == null)
                {
                    Response.Redirect("GestionMedicos.aspx");
                    return;
                }

                idMedico = (int)Session["IdMedicoEliminar"];

                MedicoNegocio negocio = new MedicoNegocio();
                Dominio.Medico medico = negocio.BuscarPorId(idMedico);

                if (medico == null)
                {
                    Session.Remove("IdMedicoEliminar");
                    Response.Redirect("GestionMedicos.aspx");
                    return;
                }

                
                lblNombre.Text = medico.Nombre;
                lblApellido.Text = medico.Apellido;

                //  MOSTRAR TODAS LAS ESPECIALIDADES
                if (medico.Especialidad != null && medico.Especialidad.Any())
                    lblEspecialidad.Text = string.Join(", ", medico.Especialidad.Select(x => x.Descripcion));
                else
                    lblEspecialidad.Text = "Sin especialidad";
            }
        }

       
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (Session["IdMedicoEliminar"] == null)
            {
                Response.Redirect("GestionMedicos.aspx");
                return;
            }

            idMedico = (int)Session["IdMedicoEliminar"];

            try
            {
                MedicoNegocio negocio = new MedicoNegocio();

                //  ELIMINACIÓN SEGURA (lógica)
                negocio.Eliminar(idMedico);

                Session.Remove("IdMedicoEliminar");
                Response.Redirect("GestionMedicos.aspx");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "error",
                    $"alert('Error al eliminar: {ex.Message}');", true
                );
            }
        }

        
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("IdMedicoEliminar");
            Response.Redirect("GestionMedicos.aspx");
        }
    }
}