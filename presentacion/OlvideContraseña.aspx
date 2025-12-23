<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="OlvideContraseña.aspx.cs" Inherits="presentacion.OlvideContraseña" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container d-flex justify-content-center align-items-center" style="min-height: 80vh;">
        <div class="card shadow p-4" style="width: 100%; max-width: 400px;">
            <h3 class="text-center mb-3">Recuperar contraseña</h3>

            <label class="form-label">Usuario</label>
            <asp:TextBox ID="txtUsuario" CssClass="form-control mb-3" runat="server"></asp:TextBox>

            <asp:Button ID="btnEnviar" CssClass="btn btn-primary w-100" runat="server" Text="Enviar contraseña" OnClick="btnEnviar_Click" />
            <br />
            <asp:Button ID="btnVolver" CssClass="btn btn-secondary w-100" runat="server" Text="Volver" OnClick="btnVolver_Click" />


            <div class="mt-3 text-center">
                <asp:Label ID="lblMensaje" CssClass="text-danger" runat="server"></asp:Label>
            </div>
        </div>
    </div>

</asp:Content>
