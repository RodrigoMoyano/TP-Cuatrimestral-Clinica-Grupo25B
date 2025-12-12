using System;
using System.Collections.Generic;
using System.Linq;
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
            {
                CargarEspecialidades();
                CargarGrilla();
            }
        }

        
        //  CARGA DE ESPECIALIDADES PARA EL FILTRO
     
        private void CargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            var lista = negocio.Listar();

            ddlEspecialidad.Items.Clear();
            ddlEspecialidad.Items.Add(new ListItem("Todas", "0"));

            foreach (var esp in lista)
                ddlEspecialidad.Items.Add(new ListItem(esp.Descripcion, esp.Id.ToString()));
        }

        //  CARGA DE LA GRILLA
        
        private void CargarGrilla()
        {
            MedicoNegocio negocio = new MedicoNegocio();
            List<Medico> lista;

            // Primero traemos según estado
            if (ddlEstado.SelectedValue == "0") // Inactivos
                lista = negocio.ListarInactivos();
            else if (ddlEstado.SelectedValue == "2") // Todos
                lista = negocio.ListarTodos();
            else // Activos
                lista = negocio.Listar();

            // despues aplicamos filtros de nombre y especialidad
            lista = AplicarFiltros(lista);

            gvMedicos.DataSource = lista;
            gvMedicos.DataBind();
        }

      
        //      FILTROS APLICADOS
     
        private List<Medico> AplicarFiltros(List<Medico> lista)
        {
          
            // FILTRO POR NOMBRE
            
            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                string texto = txtBuscar.Text.ToLower();
                lista = lista.Where(m =>
                    (!string.IsNullOrEmpty(m.Nombre) && m.Nombre.ToLower().Contains(texto)) ||
                    (!string.IsNullOrEmpty(m.Apellido) && m.Apellido.ToLower().Contains(texto))
                ).ToList();
            }

            
            // FILTRO POR ESPECIALIDAD
            if (ddlEspecialidad.SelectedValue != "0")
            {
                lista = lista.Where(m =>
                    m.Especialidad != null &&
                    m.Especialidad.Any(e => e.Id.ToString() == ddlEspecialidad.SelectedValue)
                ).ToList();
            }

            return lista;
        }

        
        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        // BOTÓN AGREGAR
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
            if (e.CommandName == "CambiarEstado")
            {
                int idMedico = Convert.ToInt32(e.CommandArgument);

                MedicoNegocio negocio = new MedicoNegocio();
                Medico medico = negocio.BuscarPorId(idMedico);

                if (medico != null)
                {
                    bool nuevoEstado = !medico.Activo;   // si estaba activo entonces inactivo, y viceversa
                    negocio.CambiarEstado(idMedico, nuevoEstado);
                    CargarGrilla();                      // refresca la grilla para ver el cambio
                }

                return; // salimos para no pasar al bloque de abajo
            }


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