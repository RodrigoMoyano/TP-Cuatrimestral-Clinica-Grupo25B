<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="AgregarAdmin.aspx.cs" Inherits="presentacion.AgregarAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>Crear Administrador</h2>

    <asp:Label runat="server" Text="Nombre de Usuario:" />
    <asp:TextBox ID="txtUsuario" CssClass="form-control" runat="server" />
    <asp:RequiredFieldValidator CssClass="validacion" ErrorMessage="Ingrese un usuario." ValidationGroup="admin" Display="Dynamic" ControlToValidate="txtUsuario" runat="server" />

    <br />

    <asp:Label runat="server" Text="Clave:" />
    <asp:TextBox ID="txtClave" CssClass="form-control" TextMode="Password" runat="server" />
    <asp:RequiredFieldValidator CssClass="validacion" ErrorMessage="Ingrese una contraseña." ValidationGroup="admin" Display="Dynamic" ControlToValidate="txtClave" runat="server" />
    <asp:RequiredFieldValidator CssClass="validacion" ErrorMessage="Minimo 8 caracteres" ValidationGroup="admin" Display="Dynamic" ControlToValidate="txtClave" runat="server" />

    <br />

    <asp:Button ID="btnGuardar" CssClass="btn btn-primary mt-3" Text="Crear Admin" runat="server" OnClick="btnGuardar_Click" />
    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger mt-1" OnClick="btnCancelar_Click" />

    <br />

    <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false" />

</asp:Content>
