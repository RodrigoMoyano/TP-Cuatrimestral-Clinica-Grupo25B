<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="GestionPaciente.aspx.cs" Inherits="presentacion.GestionPaciente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    

    
   <asp:Button ID="btnIrRegistro" CssClass="btn btn-success mb-3"
    runat="server" Text="Agregar Paciente"
    OnClick="btnIrRegistro_Click" />


    <div class="col-md-4">
    <asp:DropDownList ID="ddlFiltro" runat="server" AutoPostBack="true"
        OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" CssClass="form-select">
        <asp:ListItem Text="Todos" Value="todos" />
        <asp:ListItem Text="Activos" Value="activos" />
        <asp:ListItem Text="Inactivos" Value="inactivos" />
    </asp:DropDownList>
    </div>

<asp:GridView ID="gvPacientes" runat="server"
    CssClass="table table-bordered table-striped"
    AutoGenerateColumns="False"
    DataKeyNames="Id"
    OnRowCommand="gvPacientes_RowCommand">

    <Columns>

        <asp:BoundField DataField="Id" HeaderText="ID" />
        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
        <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
        <asp:BoundField DataField="Dni" HeaderText="DNI" />
        <asp:BoundField DataField="Email" HeaderText="Email" />

       
        <asp:TemplateField HeaderText="Estado">
            <ItemTemplate>
                <%# (Eval("Usuario") != null && (bool?)Eval("Usuario.Activo") == true)
                        ? "Activo"
                        : "Inactivo" %>
            </ItemTemplate>
        </asp:TemplateField>

        
        <asp:ButtonField 
            Text="Editar" 
            CommandName="Editar"
            ControlStyle-CssClass="btn btn-primary btn-sm" />

    </Columns>

</asp:GridView>

<asp:Label ID="lblMensaje" runat="server" CssClass="text-danger"></asp:Label>
</asp:Content>