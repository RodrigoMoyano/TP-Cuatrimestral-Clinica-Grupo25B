<%@ Page Title="Editar Médico" Language="C#" MasterPageFile="~/MiMaster.Master"
    AutoEventWireup="true" CodeBehind="EditarMedico.aspx.cs"
    Inherits="presentacion.EditarMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3 class="mb-4">Editar Médico</h3>

    <asp:ValidationSummary ID="vsErrores" runat="server"
        CssClass="text-danger mb-3"
        ValidationGroup="Medico" />

    <div class="card p-4 shadow-sm">

        <asp:HiddenField ID="hfIdMedico" runat="server" />

        
        <div class="mb-3">
            <label>Nombre</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator ControlToValidate="txtNombre"
                ErrorMessage="Ingrese un nombre" CssClass="text-danger"
                ValidationGroup="Medico" runat="server" />
        </div>

        <div class="mb-3">
            <label>Apellido</label>
            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator ControlToValidate="txtApellido"
                ErrorMessage="Ingrese un apellido" CssClass="text-danger"
                ValidationGroup="Medico" runat="server" />
        </div>

        <div class="mb-3">
            <label>Matrícula</label>
            <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" />
        </div>

        <div class="mb-3">
            <label>Teléfono</label>
            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
        </div>

        <div class="mb-3">
            <label>Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
        </div>

        <div class="mb-3">
            <label>Especialidad</label>
            <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator ControlToValidate="ddlEspecialidad"
                InitialValue="0" ErrorMessage="Seleccione una especialidad"
                CssClass="text-danger" ValidationGroup="Medico" runat="server" />
        </div>

    </div>

    <div class="mt-3">
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios"
            CssClass="btn btn-primary me-2"
            ValidationGroup="Medico"
            OnClick="btnGuardar_Click" />

        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
            CssClass="btn btn-secondary"
            CausesValidation="false"
            OnClick="btnCancelar_Click" />
    </div>

</asp:Content>
