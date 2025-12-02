<%@ Page Title="Eliminar Médico" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="EliminarMedico.aspx.cs" Inherits="presentacion.EliminarMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3 class="mb-4">Eliminar Médico</h3>

    <div class="card p-4 shadow-sm">
        <p>¿Está seguro que desea eliminar el siguiente médico?</p>

        <p><strong>Nombre:</strong> <asp:Label ID="lblNombre" runat="server" /></p>
        <p><strong>Apellido:</strong> <asp:Label ID="lblApellido" runat="server" /></p>
        <p><strong>Especialidad:</strong> <asp:Label ID="lblEspecialidad" runat="server" /></p>
    </div>

    <asp:Button ID="btnConfirmar" runat="server" Text="Eliminar" CssClass="btn btn-danger mb-3" OnClick="btnConfirmar_Click" />
    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary mb-3" OnClick="btnCancelar_Click" />

</asp:Content>