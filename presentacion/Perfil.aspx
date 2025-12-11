<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="presentacion.Perfil" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4" style="max-width:600px;">
    <h3 class="mb-3">Mi Perfil</h3>

    <!-- Datos NO editables -->
    <div class="mb-3">
        <label class="form-label">Nombre</label>
        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" ReadOnly="true" />
    </div>

    <div class="mb-3">
        <label class="form-label">Apellido</label>
        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" ReadOnly="true" />
    </div>

    <div class="mb-3">
        <label class="form-label">DNI</label>
        <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" ReadOnly="true" />
    </div>

    <div class="card mb-4">
        <div class="card-header">Cobertura</div>
        <div class="card-body row g-3">

            <div class="col-md-6">
                <label class="form-label fw-bold">Tipo de Cobertura</label>
                <asp:Label ID="lblTipoCobertura" runat="server" CssClass="form-control bg-light" />
            </div>

            <div class="col-md-6">
                <label class="form-label fw-bold">Obra Social</label>
                <asp:Label ID="lblObraSocial" runat="server" CssClass="form-control bg-light" />
            </div>

        </div>
    </div>


    <!-- DATOS EDITABLES -->

    <div class="mb-3">
        <label class="form-label">Nombre de Usuario</label>
        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" />
        <asp:Label ID="lblErrorUsuario" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
    </div>

    <div class="mb-3">
        <label class="form-label">Teléfono</label>
        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
        <asp:Label ID="lblErrorTelefono" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
    </div>

    <div class="mb-3">
        <label class="form-label">Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
        <asp:Label ID="lblErrorEmail" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
    </div>

    <div class="mb-3">
        <label class="form-label">Contraseña</label>

        <div class="input-group">
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />

            <button type="button" class="btn btn-outline-secondary" onclick="togglePassword()">
                <span id="btnText">Mostrar</span>
            </button>
        </div>

        <asp:Label ID="lblErrorPassword" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
    </div>

    <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" Text="Guardar cambios" OnClick="btnGuardar_Click" />
    <asp:Button ID="btnVolver" runat="server" CssClass="btn btn-secondary ms-2" Text="Volver"
    OnClick="btnVolver_Click" CausesValidation="false" />

    <asp:Label ID="lblMensaje" runat="server" CssClass="mt-3 d-block"></asp:Label>

</div>
    <script>
        function togglePassword() {
            const txt = document.getElementById("<%= txtPassword.ClientID %>");
            const btnText = document.getElementById("btnText");

            if (txt.type === "password") {
                txt.type = "text";
                btnText.innerText = "Ocultar";
            } else {
                txt.type = "password";
                btnText.innerText = "Mostrar";
            }
        }
    </script>

</asp:Content>