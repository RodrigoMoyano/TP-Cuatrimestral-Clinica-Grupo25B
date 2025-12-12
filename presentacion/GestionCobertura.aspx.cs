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
    public partial class GestionCobertura : PaginaAdmin
    {
        CoberturaNegocio negocio = new CoberturaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCoberturas();
            }
        }

        private void CargarCoberturas()
        {
            var lista = negocio.Listar()
                                .Where(c => c.Tipo == "Obra Social") 
                                .ToList();

            gvCoberturas.DataSource = lista;
            gvCoberturas.DataBind();
        }

        protected void gvCoberturas_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvCoberturas.PageIndex = e.NewPageIndex;
            CargarCoberturas();
        }

        protected void btnAgregarNueva_Click(object sender, EventArgs e)
        {
            hfIdCobertura.Value = "0";
            pnlFormulario.Visible = true;
            lblTituloFormulario.InnerText = "Agregar Obra Social";
            txtTipo.Text = "Obra Social";  
            txtTipo.Enabled = false;
            txtNombreObraSocial.Text = "";
            txtPlanCobertura.Text = "";
        }

        protected void gvCoberturas_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Cobertura c = negocio.Listar().Find(x => x.Id == id);
                if (c != null)
                {
                    pnlFormulario.Visible = true;
                    lblTituloFormulario.InnerText = "Editar Obra Social";
                    txtTipo.Text = "Obra Social";
                    txtTipo.Enabled = false;
                    hfIdCobertura.Value = c.Id.ToString();
                    txtNombreObraSocial.Text = c.NombreObraSocial;
                    txtPlanCobertura.Text = c.PlanCobertura;
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                negocio.Eliminar(id);
                CargarCoberturas();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            lblError.Visible = false; 

            
            if (string.IsNullOrWhiteSpace(txtNombreObraSocial.Text))
            {
                lblError.Text = "Debe ingresar un nombre de obra social.";
                lblError.Visible = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPlanCobertura.Text))
            {
                lblError.Text = "Debe ingresar un plan de cobertura.";
                lblError.Visible = true;
                return;
            }

            try
            {
                Cobertura c = new Cobertura
                {
                    Id = int.Parse(hfIdCobertura.Value),
                    Tipo = "Obra Social",
                    NombreObraSocial = txtNombreObraSocial.Text.Trim(),
                    PlanCobertura = txtPlanCobertura.Text.Trim()
                };

                if (c.Id == 0)
                    negocio.Agregar(c);
                else
                    negocio.Modificar(c);

                pnlFormulario.Visible = false;
                CargarCoberturas();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al guardar: " + ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlFormulario.Visible = false;
        }

        protected void gvCoberturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }
    }
}