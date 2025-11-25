<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="presentacion.LogIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container d-flex flex-column align-items-center justify-content-center" style="min-height: 90vh;">

        <!-- Imagen -->
        <div class="card-shadow">
            <div class="card-image-container">
                <img class="card-image" src="~/imagenes/3662541.jpg" runat="server" alt="imagen clínica">
            </div>

            <!-- Login -->
            <div class="card-body">
                <h4 class="card-title text-center mb-4">Inicio de Sesión</h4>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Usuario</label>
                    <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="validacion" ValidationGroup="LoginGroup" ErrorMessage="Ingresa el usuario" ControlToValidate="txtEmail" runat="server" />
                </div>

                <div class="mb-3">
                    <label for="txtPassword" class="form-label">Contraseña</label>
                    <asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="validacion" ValidationGroup="LoginGroup" ErrorMessage="Ingresa la contraseña" ControlToValidate="txtPassword" runat="server" />
                </div>

                <div class="text-center">
                    <asp:Button ID="btnIngresar" CssClass="btn btn-primary w-100" OnClick="btnIngresar_Click" runat="server" Text="Ingresar" />
                </div>
                 <!-- Corroborar usuario -->
                <div class="text-center mt-3">
                    <asp:Label ID="lblError" runat="server" 
                        ForeColor="Red" 
                        Visible="false" 
                        Font-Bold="true">
                    </asp:Label>
                </div>


            <asp:Button ID="btnRegistrarse" ValidationGroup="LoginGroup" CssClass="btn btn-outline-secondary w-100" OnClick="btnRegistrarse_Click" runat="server" Text="Registrarse" />
            </div>
        </div>
    </div>

</asp:Content>

