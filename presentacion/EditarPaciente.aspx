<%@ Page Title="Editar Paciente" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="EditarPaciente.aspx.cs" Inherits="presentacion.EditarPaciente" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <title>Editar Paciente</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-4">
        <h2 class="mb-4">Editar Paciente</h2>

        <!-- ================= DATOS DE USUARIO ================= -->
        <div class="card mb-4">
            <div class="card-header">Datos de Usuario</div>
            <div class="card-body row g-3">
                <div class="col-md-2">
                    <label class="form-label">ID Usuario</label>
                    <asp:TextBox ID="txtIdUsuario" runat="server" 
                        CssClass="form-control bg-light" 
                        ReadOnly="true">
                    </asp:TextBox>
                </div>
                <div class="col-md-5">
                    <label class="form-label">Nombre de Usuario</label>
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-5">
                    <label class="form-label">Contraseña</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtClave" runat="server" TextMode="Password" CssClass="form-control" />
                        <button type="button" class="btn btn-outline-secondary" onclick="togglePassword()">
                            <i id="iconEye" class="fa fa-eye"></i>
                        </button>
                    </div>
                </div>

            </div>
        </div>

        <!-- ================= DATOS DEL PACIENTE ================= -->
        <div class="card mb-4">
            <div class="card-header">Datos del Paciente</div>
            <div class="card-body row g-3">

                <div class="col-md-2">
                    <label class="form-label">ID Paciente</label>
                    <asp:TextBox ID="txtIdPaciente" runat="server" 
                        CssClass="form-control bg-light" 
                        ReadOnly="true">
                    </asp:TextBox>
                </div>

                <div class="col-md-4">
                    <label class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                </div>

                <div class="col-md-4">
                    <label class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                </div>

                <div class="col-md-2">
                    <label class="form-label">DNI</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" />
                </div>

                <div class="col-md-4">
                    <label class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                </div>

                <div class="col-md-4">
                    <label class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
                </div>

                <div class="col-md-4">
                    <label class="form-label"> Estado </label>
                    <asp:DropDownList ID="ddlActivoUsuario" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Activo" Value="true" />
                        <asp:ListItem Text="Inactivo" Value="false" />
                    </asp:DropDownList>
                </div>

            </div>
        </div>

        <!-- ================= COBERTURA ================= -->
        <div class="mb-3">
            <label class="form-label fw-bold">Cobertura</label>
            <div class="col-md-4">
                <asp:DropDownList 
                    ID="ddlCobertura" 
                    runat="server" 
                    CssClass="form-select"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlCobertura_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            
            <div class="col-md-4">
                <asp:DropDownList 
                    ID="ddlObraSocial" 
                    runat="server" 
                    CssClass="form-select"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlObraSocial_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>

        <!-- BOTONES -->
        <div class="mb-3">
            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" Text="Guardar cambios"
                OnClick="btnGuardar_Click" />

            <asp:Button ID="btnVolver" runat="server" CssClass="btn btn-secondary ms-2" Text="Volver"
                OnClick="btnVolver_Click" CausesValidation="false" />

            <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger ms-3" />
        </div>

    </div>
    <script>
    function togglePassword() {
        const txt = document.getElementById("<%= txtClave.ClientID %>");
        const icon = document.getElementById("iconEye");

        if (txt.type === "password") {
            txt.type = "text";
            icon.classList.remove("fa-eye");
            icon.classList.add("fa-eye-slash");
        } else {
            txt.type = "password";
            icon.classList.remove("fa-eye-slash");
            icon.classList.add("fa-eye");
        }
    }
    </script>

</asp:Content>