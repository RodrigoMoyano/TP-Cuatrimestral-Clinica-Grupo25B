<%@ Page Title="Gestión de Médicos" Language="C#" MasterPageFile="~/MiMaster.Master"
    AutoEventWireup="true" CodeBehind="GestionMedicos.aspx.cs"
    Inherits="presentacion.GestionMedicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3 class="mb-4">Gestión de Médicos</h3>

    <asp:Button ID="btnAgregar" runat="server" Text="Agregar Médico"
        CssClass="btn btn-primary mb-3"
        OnClick="btnAgregar_Click" />

    <asp:GridView ID="gvMedicos" runat="server" AutoGenerateColumns="False"
        AllowPaging="true" PageSize="10"
        OnPageIndexChanging="gvMedicos_PageIndexChanging"
        OnRowCommand="gvMedicos_RowCommand"
        DataKeyNames="Id" CssClass="table table-striped">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="ID" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
            <asp:BoundField DataField="Matricula" HeaderText="Matrícula" />
            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
            <asp:BoundField DataField="Email" HeaderText="Email" />

            <asp:TemplateField HeaderText="Especialidad">
    <ItemTemplate>
        <%# FormatearEspecialidad(Eval("Especialidad")) %>
    </ItemTemplate>
</asp:TemplateField>

            <asp:ButtonField CommandName="Editar" Text="Editar" ButtonType="Button" />
            <asp:ButtonField CommandName="Eliminar" Text="Eliminar" ButtonType="Button" />
            <asp:ButtonField CommandName="Detalle" Text="Ver" ButtonType="Button" />
        </Columns>
    </asp:GridView>

</asp:Content>