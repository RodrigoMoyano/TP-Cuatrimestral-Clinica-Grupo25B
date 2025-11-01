<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="presentacion.Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="simple-menu-bg">
        <div class="container-fluid">
            <div class="header-with-image">
                <h1 class="text-center mb-4">Gestor de Turnos</h1>
                <img src="Imagenes/calendarioMenu.png" runat="server" alt="Sistema de Turnos" class="header-image" />
            </div>
            
            <div class="text-center">
                <footer class="fs-5 text-muted mt-3">Sistema profesional de gestión de citas médicas</footer>
            </div>
        </div>
    </div>

</asp:Content>
