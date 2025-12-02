    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObservacionesTurno.aspx.cs"
    MasterPageFile="~/MiMaster.Master"
    Inherits="Presentacion.ObservacionesTurno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- TÍTULO CENTRADO -->
    <h3 class="text-center mb-4" style="font-size: 32px; font-weight: bold;">
        Observaciones del Turno
    </h3>

    <!-- CARD DETALLES -->
    <div class="card shadow-sm mb-4">
        <div class="card-header bg-primary text-white fw-bold">
            Datos del Turno
        </div>

        <div class="card-body">

            <p class="mb-2"><asp:Label ID="lblFecha" runat="server" /></p>
            <p class="mb-2"><asp:Label ID="lblHora" runat="server" /></p>
            <p class="mb-2"><asp:Label ID="lblPaciente" runat="server" /></p>
            <p class="mb-2"><asp:Label ID="lblEspecialidad" runat="server" /></p>
            <p class="mb-2"><asp:Label ID="lblEstado" runat="server" /></p>

            <hr />

            <h5 class="mt-3">Observaciones</h5>

            <asp:Label ID="lblObservaciones" runat="server" CssClass="text-dark"></asp:Label>

            <asp:TextBox ID="txtObservaciones" runat="server"
                TextMode="MultiLine" Rows="6"
                CssClass="form-control mt-3"
                Visible="false" />

            <div class="mt-3">
                <asp:Button ID="btnEditarObs" runat="server" Text="Editar"
                    CssClass="btn btn-warning me-2"
                    OnClick="btnEditarObs_Click" />

                <asp:Button ID="btnGuardarObs" runat="server" Text="Guardar"
                    CssClass="btn btn-success me-2"
                    Visible="false"
                    OnClick="btnGuardarObs_Click" />

                <asp:Button ID="btnCancelarEdicion" runat="server" Text="Cancelar"
                    CssClass="btn btn-secondary"
                    Visible="false"
                    OnClick="btnCancelarEdicion_Click" />
            </div>

        </div>
    </div>


    <!-- CARD ACCIONES -->
<div class="card shadow-sm mt-4">
    <div class="card-header bg-light text-dark fw-bold">
        Acciones
    </div>

    <div class="card-body">

        <asp:Button ID="btnReprogramar" runat="server"
            Text="Reprogramar"
            CssClass="btn btn-outline-secondary me-2 mb-2"
            OnClick="btnReprogramar_Click" />

        <asp:Button ID="btnCerrar" runat="server"
            Text="Cerrar Turno"
            CssClass="btn btn-outline-secondary me-2 mb-2"
            OnClick="btnCerrar_Click" />

        <asp:Button ID="btnNoAsistio" runat="server"
            Text="No asistió"
            CssClass="btn btn-outline-secondary me-2 mb-2"
            OnClick="btnNoAsistio_Click" />

        <asp:Button ID="btnCancelar" runat="server"
            Text="Cancelar Turno"
            CssClass="btn btn-outline-secondary me-2 mb-2"
            OnClick="btnCancelar_Click" />

    </div>
</div>

    <!-- BOTÓN VOLVER (CENTRADO) -->
    <div class="text-center mt-4">
        <asp:Button 
            ID="btnVolver" 
            runat="server" 
            Text="Volver al Panel Médico"
            CssClass="btn btn-outline-primary px-4 py-2"
            OnClick="btnVolver_Click" />
    </div>

</asp:Content>