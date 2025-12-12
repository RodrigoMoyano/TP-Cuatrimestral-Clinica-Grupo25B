<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="EditarAdmin.aspx.cs" Inherits="presentacion.EditarAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3>Editar Administrador</h3>

    <div class="mb-3">
        <asp:Label Text="Nombre de usuario" runat="server" />
        <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
        <asp:RequiredFieldValidator
            ID="valNombre"
            runat="server"
            ControlToValidate="txtNombre"
            ErrorMessage="El nombre de usuario es obligatorio."
            Display="Dynamic"
            CssClass="text-danger" />
    </div>

    <div class="mb-3">
        <asp:Label Text="Nueva contraseña" runat="server" />
        <asp:TextBox ID="txtClave" CssClass="form-control" TextMode="Password" runat="server" />
    </div>

    <asp:Button ID="btnGuardar"
        Text="Guardar cambios"
        CssClass="btn btn-primary"
        runat="server"
        OnClick="btnGuardar_Click" />

    <asp:Label ID="lblMensaje" CssClass="text-danger fw-bold mt-3" runat="server" />


</asp:Content>
