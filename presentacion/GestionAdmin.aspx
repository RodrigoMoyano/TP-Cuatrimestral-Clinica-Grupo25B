<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="GestionAdmin.aspx.cs" Inherits="presentacion.GestionAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>Administradores</h2>

    <asp:Button ID="btnAgregarAdmin" runat="server"
        Text="Agregar Administrador"
        CssClass="btn btn-success"
        OnClick="btnAgregarAdmin_Click" />

    <hr />

    <div class="mb-3">
        <label class="form-label fw-bold">Estado</label>
        <asp:DropDownList ID="ddlEstado" CssClass="form-select" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
            <asp:ListItem Text="Todos" Value="TODOS"></asp:ListItem>
            <asp:ListItem Text="Activos" Value="1"></asp:ListItem>
            <asp:ListItem Text="Inactivos" Value="0"></asp:ListItem>
        </asp:DropDownList>
    </div>

    <hr />

    <asp:GridView ID="dgvAdmin" runat="server"
        AutoGenerateColumns="False"
        CssClass="table table-striped"
        OnRowCommand="dgvAdmin_RowCommand">

        <Columns>
            <asp:BoundField DataField="Id" HeaderText="ID" />
            <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />

            <asp:TemplateField HeaderText="Estado">
                <ItemTemplate>
                    <%# (bool)Eval("Activo") ? "Activo" : "Inactivo" %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>

                    <!--Editar -->
                    <asp:Button ID="btnEditar" runat="server"
                        Text="Editar"
                        CommandName="EditarAdmin"
                        CommandArgument='<%# Eval("Id") %>'
                        CssClass="btn btn-primary btn-sm"/>

                    <!--Activar -Desactivar-->
                    <asp:Button ID="btnEstado" runat="server"
                        Text='<%# (bool)Eval("Activo") ? "Desactivar" : "Activar" %>'
                        CommandName='<%# (bool)Eval("Activo") ? "Desactivar" : "Activar" %>'
                        CommandArgument='<%# Eval("Id") %>'
                        CssClass="btn btn-warning btn-sm" />

                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>

</asp:Content>
