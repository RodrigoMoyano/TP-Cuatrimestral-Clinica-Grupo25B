<%@ Page Title="Agregar Especialidad" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="AgregarEspecialidad.aspx.cs" Inherits="presentacion.AgregarEspecialidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-4" style="max-width:600px;">
        <h3 class="mb-3">Agregar Especialidad</h3>

        <div class="mb-3">
            <label class="form-label">Nombre de la Especialidad</label>
            <asp:TextBox ID="txtDescripcion" CssClass="form-control" runat="server"></asp:TextBox>
        </div>

        <div class="mb-3">
            <label class="form-label">URL de Imagen (opcional)</label>
            <asp:TextBox ID="txtImagen" CssClass="form-control" runat="server"
                         placeholder="Pegue aquí la URL de la imagen"></asp:TextBox>

        </div>

        <div class="mb-3">
            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary"
                        Text="Guardar" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-secondary ms-2"
                        Text="Cancelar" OnClick="btnCancelar_Click" CausesValidation="false" />
        </div>
        <div class="alert alert-success" id="divExito" runat="server" visible="false">
            ¡Especialidad registrada con éxito!
        </div>

        <asp:Label ID="lblMensaje" runat="server" CssClass="mt-2"></asp:Label>
    </div>

</asp:Content>