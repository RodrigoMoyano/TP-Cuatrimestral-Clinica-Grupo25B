<%@ Page Title="Gestión de Médicos" Language="C#" MasterPageFile="~/MiMaster.Master"
    AutoEventWireup="true" CodeBehind="GestionMedicos.aspx.cs"
    Inherits="presentacion.GestionMedicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3 class="mb-4">Gestión de Médicos</h3>

    <div class="card p-3 mb-3">
        <div class="row">

           
            <div class="col-md-4 mb-2">
                <label><strong>Buscar por nombre:</strong></label>
                <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"
                    AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" />
            </div>

          
            <div class="col-md-4 mb-2">
                <label><strong>Especialidad:</strong></label>
                <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged" />
            </div>

          
            <div class="col-md-4 mb-2">
                <label><strong>Estado:</strong></label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                    <asp:ListItem Value="1">Activos</asp:ListItem>
                    <asp:ListItem Value="0">Inactivos</asp:ListItem>
                    <asp:ListItem Value="2">Todos</asp:ListItem>
                </asp:DropDownList>
            </div>

        </div>
    </div>

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
    <asp:BoundField DataField="EspecialidadesTexto" HeaderText="Especialidades" />

   
    <asp:TemplateField HeaderText="Estado">
        <ItemTemplate>
            <%# (bool)Eval("Activo") ? "Activo" : "Inactivo" %>
        </ItemTemplate>
    </asp:TemplateField>

    
    <asp:TemplateField HeaderText="Acción">
        <ItemTemplate>
            <asp:Button ID="btnCambiarEstado" runat="server"
                Text='<%# (bool)Eval("Activo") ? "Inactivar" : "Activar" %>'
                CommandName="CambiarEstado"
                CommandArgument='<%# Eval("Id") %>'
                CssClass='<%# (bool)Eval("Activo") ? "btn btn-warning btn-sm" : "btn btn-success btn-sm" %>' />
        </ItemTemplate>
    </asp:TemplateField>

    
    <asp:ButtonField CommandName="Editar" Text="Editar" ButtonType="Button" />
    <asp:ButtonField CommandName="Eliminar" Text="Eliminar" ButtonType="Button" />
    <asp:ButtonField CommandName="Detalle" Text="Ver" ButtonType="Button" />
</Columns>
    </asp:GridView>

</asp:Content>