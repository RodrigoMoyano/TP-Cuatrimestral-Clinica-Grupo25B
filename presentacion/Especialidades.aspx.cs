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
    public partial class Especialidades : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            Usuario usuario = (Usuario)Session["Usuario"];

            if (!IsPostBack)
                CargarEspecialidades();
        }


        private void CargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            repEspecialidades.DataSource = negocio.Listar();
            repEspecialidades.DataBind();
        }


        public string GetImagenPorEspecialidad(string descripcion)
        {
            descripcion = descripcion.ToLower();

            if (descripcion.Contains("clínica") || descripcion.Contains("clinica"))
                return "https://clinicajaimeicatarroja.com/sites/default/files/imagen-servicio-principal/medicina-general.jpg";

            if (descripcion.Contains("odont"))
                return "https://tse4.mm.bing.net/th/id/OIP.447fweDx0FOExM1rLHPbIwHaE8?cb=ucfimg2ucfimg=1&w=950&h=634&rs=1&pid=ImgDetMain&o=7&rm=3";

            if (descripcion.Contains("dermat"))
                return "https://media.istockphoto.com/id/1152216276/es/foto/dermat%C3%B3logo-en-guantes-de-l%C3%A1tex-que-sostienen-dermatoscopio-mientras-examina-a-un-paciente.jpg?s=612x612&w=0&k=20&c=gnfv_Mns1FFwSOJaTvHC3OwKeTa6Du0s-SrNrzzWy6k=";

            if (descripcion.Contains("cardio"))
                return "https://tse1.explicit.bing.net/th/id/OIP.JTs0bo1Z1cElFZ5kdnmiQAHaFB?cb=ucfimg2ucfimg=1&rs=1&pid=ImgDetMain&o=7&rm=3";

            // Imagen por defecto
            return "https://www.versatilis.com.br/wp-content/uploads/2023/04/patient-nurse-sitting-reception-desk-talking-female-receptionist-about-disease-diagnosis-healthcare-support-diverse-people-working-health-center-registration-counter-2-scaled.jpg";
        }

    }
}
