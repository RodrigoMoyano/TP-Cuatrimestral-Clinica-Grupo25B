using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class GestionMedicos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarGrilla();
        }

        private void CargarGrilla()
        {
            MedicoNegocio negocio = new MedicoNegocio();
            gvMedicos.DataSource = negocio.Listar();
            gvMedicos.DataBind();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgregarMedico.aspx");
        }

        // 🔹 PAGINACIÓN
        protected void gvMedicos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMedicos.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        // 🔹 EDITAR / ELIMINAR / DETALLE (compatible con paginación)
        protected void gvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Solo procesamos las acciones que usan DataKeys
            if (e.CommandName == "Editar" ||
                e.CommandName == "Eliminar" ||
                e.CommandName == "Detalle")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                // ✅ Accedemos directamente al DataKey sin usar gvMedicos.Rows[index]
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
        protected string FormatearEspecialidad(object especialidadesObj)
        {
            if (especialidadesObj == null)
                return "Sin especialidad";

            var lista = especialidadesObj as List<Dominio.Especialidad>;
            if (lista == null || lista.Count == 0)
                return "Sin especialidad";

            return lista[0].Descripcion; // o String.Join(", ", lista.Select(x => x.Descripcion))
        }
    }
}
