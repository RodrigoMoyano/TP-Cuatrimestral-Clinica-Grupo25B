<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="PedirTurno.aspx.cs" Inherits="presentacion.PedirTurno" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .turno-container {
            max-width: 700px;
            margin: 50px auto;
            background: #fff;
            padding: 30px;
            border-radius: 16px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.1);
        }
        h2 {
            color: #0d6efd;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="turno-container">
        <h2 class="text-center mb-4">📅 Pedir nuevo turno</h2>

        <!-- Especialidad -->
        <div class="mb-3">
            <label class="form-label fw-bold">Especialidad</label>
            <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select" AutoPostBack="true"
                OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged"></asp:DropDownList>
        </div>

        <!-- Médico -->
        <div class="mb-3">
            <label class="form-label fw-bold">Médico</label>
            <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select" AutoPostBack="true"
                OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged"></asp:DropDownList>
        </div>

        <!-- Fecha -->
        <div class="mb-3">
            <label class="form-label fw-bold">Fecha</label>
            <asp:Calendar ID="calFecha" runat="server" CssClass="border rounded p-2" OnSelectionChanged="calFecha_SelectionChanged"></asp:Calendar>
        </div>

        <!-- Horarios -->
        <div class="mb-3">
            <label class="form-label fw-bold">Horario disponible</label>
            <asp:DropDownList ID="ddlHorario" runat="server" CssClass="form-select"></asp:DropDownList>
        </div>

        <!-- Confirmar -->
        <div class="text-center mt-4">
            <asp:Button ID="btnConfirmar" runat="server" CssClass="btn btn-primary px-4" Text="Confirmar turno" OnClick="btnConfirmar_Click" />
        </div>
    </div>
</asp:Content>