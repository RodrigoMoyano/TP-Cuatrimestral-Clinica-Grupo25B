<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="presentacion.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container d-flex justify-content-center aling-items-center min-vh-100 py-5">
        <div class="card card-shadow lg p-4 card-registro">

            <div class="text-center mb-4">
                <h2 class="fw-bold text-primary">Registro de Paciente</h2>
                <p class="text-muted">Complete sus datos para generar su nueva cuenta:</p>
            </div>

            <div class="row g-4">
                <div class="col-mb-6 borde-derecho-md">
                    <h5 class="registro-titulo-seccion">👤 Datos de Cuenta</h5>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Nombre de Usuario</label>
                        <asp:TextBox ID="txtNombreUsuario" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="validacion" ErrorMessage="Ingrese un usuario." Display="Dynamic" ControlToValidate="txtNombreUsuario" runat="server" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Contraseña</label>
                        <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="validacion" ErrorMessage="Ingrese una contraseña." Display="Dynamic" ControlToValidate="txtPassword" runat="server" />
                        <asp:RequiredFieldValidator CssClass="validacion" ErrorMessage="Minimo 8 caracteres" Display="Dynamic" ControlToValidate="txtPassword" runat="server" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Correo Electronico</label>
                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Ingresa su correo electronico." Display="Dynamic" CssClass="validacion" ControlToValidate="txtEmail" runat="server" />
                        <asp:RegularExpressionValidator CssClass="validacion" ErrorMessage="Formato de email, invalido." Display="Dynamic" ControlToValidate="txtEmail" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" runat="server" />
                    </div>

                </div>
                <div class="col-6 mb-3">
                    <h5 class="registro-titulo-seccion">📄 Datos Personales</h5>

                    <div class="row">
                        <div class="col-6 mb-3">
                            <label class="form-label fw-bold">Nombre</label>
                            <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="validacion" ErrorMessage="Ingrese su nombre." Display="Dynamic" ControlToValidate="txtNombre" runat="server" />
                        </div>

                        <div class="col-6 mb-3">
                            <label class="form-label fw-bold">Apellido</label>
                            <asp:TextBox ID="txtApellido" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="validacion" ErrorMessage="Ingrese su apellido." Display="Dynamic" ControlToValidate="txtApellido" runat="server" />
                        </div>
                    </div>


                    <div class="mb-3">
                        <label class="form-label fw-bold">DNI</label>
                        <asp:TextBox ID="txtDni" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="validacion" ErrorMessage="Ingrese su DNI." Display="Dynamic" ControlToValidate="txtDni" runat="server" />
                        <asp:RegularExpressionValidator CssClass="validacion" ErrorMessage="Solo numeros, entre 8 y 10 digitos." Display="Dynamic" ValidationExpression="^\d{8,10}$" ControlToValidate="txtDni" runat="server" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Telefono</label>
                        <asp:TextBox ID="txtTelefono" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Ingrese su numero de telefono." CssClass="validacion" Display="Dynamic" ControlToValidate="txtTelefono" runat="server" />
                        <asp:RegularExpressionValidator CssClass="validacion" ErrorMessage="Ingrese un numero de telefono valido." ValidationExpression="^\d{8,10}$" ControlToValidate="txtTelefono" runat="server" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Cobertura Médica</label>
                        <asp:DropDownList ID="ddlCobertura" CssClass="form-selected" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="d-grid gap-2 mt-4 pt-3 border-top">
                <asp:Button ID="btnAceptarRegistro" CssClass="btn btn-primary mt-3" OnClick="btnAceptarRegistro_Click" runat="server" Text="Aceptar" />
                <asp:Label ID="lblMensaje" runat="server" CssClass="fw-bold text-danger" Font-Bold="true" />
            </div>
        </div>
    </div>
</asp:Content>

