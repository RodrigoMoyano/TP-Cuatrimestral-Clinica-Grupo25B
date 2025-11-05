using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AccesoDatos;
using Dominio;

namespace presentacion
{
    public partial class Especialidad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspecialidades();
            }
        }

        private void CargarEspecialidades()
        {
            EspecialidadDatos datos = new EspecialidadDatos();
            var lista = datos.Listar(); 

            gvEspecialidad.DataSource = lista;
            gvEspecialidad.DataBind();
        }
    }
}