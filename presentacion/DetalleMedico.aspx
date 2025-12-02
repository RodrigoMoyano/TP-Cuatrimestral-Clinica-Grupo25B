<%@ Page Title="Detalle de Médico" Language="C#" 
    MasterPageFile="~/MiMaster.Master" 
    AutoEventWireup="true" 
    CodeBehind="DetalleMedico.aspx.cs" 
    Inherits="presentacion.DetalleMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3 class="mb-4">Detalle del Médico</h3>

    <div class="card p-4 shadow-sm">

        <p><strong>ID:</strong> <asp:Label ID="lblId" runat="server" /></p>
        <p><strong>Nombre:</strong> <asp:Label ID="lblNombre" runat="server" /></p>
        <p><strong>Apellido:</strong> <asp:Label ID="lblApellido" runat="server" /></p>
        <p><strong>Matrícula:</strong> <asp:Label ID="lblMatricula" runat="server" /></p>
        <p><strong>Email:</strong> <asp:Label ID="lblEmail" runat="server" /></p>
        <p><strong>Teléfono:</strong> <asp:Label ID="lblTelefono" runat="server" /></p>
        <p><strong>Especialidad:</strong> <asp:Label ID="lblEspecialidad" runat="server" /></p>

    </div>

    <asp:Button ID="btnVolver" runat="server" CssClass="btn btn-secondary mt-3"
        Text="Volver" OnClick="btnVolver_Click" />

</asp:Content>