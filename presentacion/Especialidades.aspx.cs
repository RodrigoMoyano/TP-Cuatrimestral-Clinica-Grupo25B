using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace presentacion
{
    public partial class Especialidades : PaginaPaciente
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

             Usuario usuario = (Usuario)Session["usuario"];

            if (usuario.Rol != null && usuario.Rol.Id == 1)
            {
                btnAgregarEspecialidad.Visible = true;
                repEspecialidades.Visible = true;

            }
            if (!IsPostBack)
                CargarEspecialidades();
            
        }

        private void CargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            repEspecialidades.DataSource = negocio.Listar();
            repEspecialidades.DataBind();
        }
        protected void btnAgregarEspecialidad_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgregarEspecialidad.aspx");
        }

        protected void repEspecialidades_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Button btnEditar = (Button)e.Item.FindControl("btnEditar");

                btnEditar.Visible = (usuario.Rol.Id == 1);
            }
        }

        protected void repEspecialidades_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                Response.Redirect($"EditarEspecialidad.aspx?Id={id}");
            }
        }

    }
}
