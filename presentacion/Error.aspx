<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="presentacion.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="error-card">
            <span class="error-icon">🚫</span>

            <h2 class="text-danger fw-bold mb-3">Acceso Denegado</h2>

            <asp:Label ID="lblMensaje" runat="server" CssClass="d-block fs-5 text-secondary mb-4" Text="Ha ocurrido un error inesperado."></asp:Label>

            <div class="d-grid gap-2">
                <asp:Button ID="btnVolver" runat="server" Text="Volver al Inicio" CssClass="btn btn-primary btn-lg" OnClick="btnVolver_Click" />
            </div>
        </div>
    </div>

</asp:Content>
