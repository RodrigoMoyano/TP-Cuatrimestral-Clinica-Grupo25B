<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="GestionCobertura.aspx.cs" Inherits="presentacion.GestionCobertura" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>Gestión de Coberturas</h3>

    <asp:Button ID="btnAgregarNueva" runat="server" Text="Agregar Cobertura" CssClass="btn btn-primary mb-3"
        OnClick="btnAgregarNueva_Click" />

    <asp:GridView ID="gvCoberturas" runat="server" AutoGenerateColumns="False"
    CssClass="table table-striped"
    OnRowCommand="gvCoberturas_RowCommand"
    OnRowDataBound="gvCoberturas_RowDataBound"
    AllowPaging="true" PageSize="10" OnPageIndexChanging="gvCoberturas_PageIndexChanging">
    <Columns>
        <asp:BoundField DataField="NombreObraSocial" HeaderText="Nombre Obra Social" />
        <asp:BoundField DataField="PlanCobertura" HeaderText="Plan" />
        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <asp:Button ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id") %>'
                    Text="Editar" CssClass="btn btn-warning btn-sm" />
                <asp:Button ID="btnEliminar" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
                    Text="Eliminar" CssClass="btn btn-danger btn-sm ms-1" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

   
    <asp:Panel ID="pnlFormulario" runat="server" CssClass="card p-3 mt-3" Visible="false">
        <h5 id="lblTituloFormulario" runat="server"></h5>

        <div class="mb-3">
            <asp:Label ID="lblTipo" runat="server" Text="Tipo"></asp:Label>
            <asp:TextBox ID="txtTipo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="mb-3">
            <asp:Label ID="lblNombre" runat="server" Text="Nombre Obra Social"></asp:Label>
            <asp:TextBox ID="txtNombreObraSocial" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="mb-3">
            <asp:Label ID="lblPlan" runat="server" Text="Plan"></asp:Label>
            <asp:TextBox ID="txtPlanCobertura" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <asp:HiddenField ID="hfIdCobertura" runat="server" />

        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success"
            OnClick="btnGuardar_Click" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary ms-2"
            OnClick="btnCancelar_Click" />
        <asp:Label ID="lblError" runat="server" CssClass="text-danger fw-bold d-block mb-3" Visible="false"></asp:Label>
    </asp:Panel>
</asp:Content>