using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class GestionMedicos : PaginaAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarGrilla();
        }

        private void CargarGrilla()
        {
            MedicoNegocio negocio = new MedicoNegocio();
            gvMedicos.DataSource = negocio.Listar();   // YA INCLUYE especialidades pero falta turnosTrabajo
            gvMedicos.DataBind();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgregarMedico.aspx");
        }

        // PAGINACIÓN
        protected void gvMedicos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMedicos.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        // EDITAR / ELIMINAR / DETALLE
        protected void gvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar" ||
                e.CommandName == "Eliminar" ||
                e.CommandName == "Detalle")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int idMedico = Convert.ToInt32(gvMedicos.DataKeys[index].Value);

                switch (e.CommandName)
                {
                    case "Editar":
                        Session["IdMedicoEditar"] = idMedico;
                        Response.Redirect("EditarMedico.aspx");
                        break;

                    case "Eliminar":
                        Session["IdMedicoEliminar"] = idMedico;
                        Response.Redirect("EliminarMedico.aspx");
                        break;

                    case "Detalle":
                        Session["IdMedicoDetalle"] = idMedico;
                        Response.Redirect("DetalleMedico.aspx");
                        break;
                }
            }
        }
    }
}