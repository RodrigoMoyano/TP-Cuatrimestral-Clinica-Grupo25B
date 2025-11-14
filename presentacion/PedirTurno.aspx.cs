using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace presentacion
{
    public partial class PedirTurno : System.Web.UI.Page
    {
        string connectionString = "Data Source=.;Initial Catalog=ClinicaDB;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) CargarEspecialidades();
        }
        private void CargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            var lista = negocio.Listar();
            ddlEspecialidad.DataSource = lista;
            ddlEspecialidad.DataTextField = "Descripcion";
            ddlEspecialidad.DataValueField = "Id";
            ddlEspecialidad.DataBind();
            ddlEspecialidad.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione --", "0"));
        }
        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idEspecialidad = int.Parse(ddlEspecialidad.SelectedValue);
            ddlMedico.Items.Clear();

            if (idEspecialidad == 0)
                return;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT M.Id, (M.Nombre + ' ' + M.Apellido) AS NombreCompleto
                         FROM Medico M
                         INNER JOIN MedicoEspecialidad ME ON M.Id = ME.IdMedico
                         WHERE ME.IdEspecialidad = @idEspecialidad";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idEspecialidad", idEspecialidad);

                con.Open();

                ddlMedico.DataSource = cmd.ExecuteReader();
                ddlMedico.DataTextField = "NombreCompleto";
                ddlMedico.DataValueField = "Id";
                ddlMedico.DataBind();

                ddlMedico.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione --", "0"));
            }
        }
        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlHorario.Items.Clear();
        }
        protected void calFecha_SelectionChanged(object sender, EventArgs e)
        {
            ddlHorario.Items.Clear();
            if (ddlMedico.SelectedValue == "0") return;
            int idMedico = int.Parse(ddlMedico.SelectedValue);
            DateTime fechaSeleccionada = calFecha.SelectedDate;
            string diaSemana = fechaSeleccionada.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT HoraInicio, HoraFin FROM TurnoTrabajo 
                                WHERE IdMedico = @idMedico AND DiaSemana = @diaSemana";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@idMedico", idMedico);
                cmd.Parameters.AddWithValue("@diaSemana", diaSemana);
                con.Open(); SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    TimeSpan inicio = (TimeSpan)dr["HoraInicio"];
                    TimeSpan fin = (TimeSpan)dr["HoraFin"];
                    for (TimeSpan hora = inicio;
                        hora < fin; hora = hora.Add(new TimeSpan(0, 30, 0)))
                    {
                        ddlHorario.Items.Add(hora.ToString(@"hh\:mm"));
                    }
                }
            }
        }
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (ddlEspecialidad.SelectedValue == "0" || ddlMedico.SelectedValue == "0" || ddlHorario.SelectedValue == "")
            {
                Response.Write("<script>alert('Por favor, complete todos los campos.');</script>");
                return;
            }
            int idPaciente = Convert.ToInt32(Session["IdPaciente"]);
            int idMedico = int.Parse(ddlMedico.SelectedValue);
            int idEspecialidad = int.Parse(ddlEspecialidad.SelectedValue);
            DateTime fecha = calFecha.SelectedDate;
            TimeSpan hora = TimeSpan.Parse(ddlHorario.SelectedValue);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Turno (IdPaciente, IdMedico, IdEspecialidad, Fecha, Hora, IdEstadoTurno) 
                                VALUES (@idPaciente, @idMedico, @idEspecialidad, @fecha, @hora, 1)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idPaciente", idPaciente);
                cmd.Parameters.AddWithValue("@idMedico", idMedico);
                cmd.Parameters.AddWithValue("@idEspecialidad", idEspecialidad);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@hora", hora);
                con.Open(); cmd.ExecuteNonQuery();
            }
            Response.Write("<script>alert(' Turno reservado con éxito'); window.location='MenuPaciente.aspx';</script>");
        }
    }
}