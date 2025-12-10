<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="EditarEspecialidad.aspx.cs" Inherits="presentacion.EditarEspecialidad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-5">
        <h2 class="text-center text-primary mb-4">Editar Especialidad</h2>

        <div class="card p-4 shadow">

            <div class="mb-3">
                <label for="txtDescripcion" class="form-label">Descripción</label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label for="txtImagen" class="form-label">URL de la Imagen</label>
                <asp:TextBox ID="txtImagen" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="mb-3 text-center">
                <asp:Image ID="imgPreview" runat="server" CssClass="img-fluid rounded shadow"
                           Width="250px" />
            </div>

            <div class="d-flex justify-content-between mt-4">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios"
                            CssClass="btn btn-success" OnClick="btnGuardar_Click" />

                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar"
                            CssClass="btn btn-danger" OnClick="btnEliminar_Click"
                             />

                <a href="Especialidades.aspx" class="btn btn-secondary">Volver</a>
            </div>
            <div class="alert alert-success" id="divExito" runat="server" visible="false">
                ¡Especialidad editada con éxito!
            </div>
            <div class="alert alert-success" id="divEliminar" runat="server" visible="false">
                ¡Especialidad eliminada con éxito!
            </div>

        </div>
    </div>

</asp:Content>