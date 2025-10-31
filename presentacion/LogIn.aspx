<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="presentacion.LogIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container d-flex flex-column align-items-center justify-content-center" style="min-height: 90vh;">

        <!-- Imagen de portada -->
        <div class="card shadow" style="max-width: 500px; width: 100%;">
            <div style="width: 100%; height: 200px; overflow: hidden;">
                <img src="~/imagenes/3662541.jpg"
                    runat="server"
                    alt="imagen clínica"
                    style="width: 100%; height: 100%; object-fit: cover;">
            </div>

            <!-- Formulario de login -->
            <div class="card-body">
                <h4 class="card-title text-center mb-4">Inicio de Sesión</h4>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Correo electrónico</label>
                    <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label for="txtPassword" class="form-label">Contraseña</label>
                    <asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
                </div>

                <div class="text-center">
                    <asp:Button ID="btnIngresar" CssClass="btn btn-primary w-100" OnClick="btnIngresar_Click" runat="server" Text="Ingresar" />
                </div>
            </div>
        </div>

    </div>

</asp:Content>

