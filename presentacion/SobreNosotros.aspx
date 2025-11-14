<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="SobreNosotros.aspx.cs" Inherits="presentacion.SobreNosotros" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" />

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2 class="text-center text-primary mb-4">Sobre Nosotros</h2>

        <p class="lead text-center">
            En <strong>Clínica Medica</strong> trabajamos cada día para ofrecer atención médica de calidad,
            priorizando el bienestar y la comodidad de nuestros pacientes. Contamos con un equipo profesional
            de médicos, especialistas y personal administrativo comprometido con brindar un servicio humano y eficiente.
        </p>

        <hr />

        <div class="text-center mt-4">
            <h4>📍 Dirección</h4>
            <p>Av. Siempre Viva 742, Buenos Aires</p>

            <h4>📞 Teléfono</h4>
            <p>(011) 4567-8910</p>

            <h4>📧 Correo electrónico</h4>
            <p><a href="mailto:contacto@clinicamedica.com">contacto@clinicamedica.com</a></p>
        </div>
    </div>
</asp:Content>