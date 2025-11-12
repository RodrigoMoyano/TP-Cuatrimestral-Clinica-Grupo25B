<%@ Page Title="Gestión de Médicos" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="GestionMedicos.aspx.cs" Inherits="presentacion.GestionMedicos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-4">

        <h3 class="mb-4"><i class="bi bi-stethoscope"></i> Gestión de Médicos</h3>

        <!-- 🔍 Filtros -->
        <div class="row mb-3">
            <div class="col-md-4">
                <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar por nombre o apellido..." />
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlEspecialidadFiltro" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Todas las especialidades" Value="" />
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlEstadoFiltro" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Todos los estados" Value="" />
                    <asp:ListItem Text="Activos" Value="1" />
                    <asp:ListItem Text="Inactivos" Value="0" />
                </asp:DropDownList>
            </div>
            <div class="col-md-2 text-end">
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-outline-primary w-100" />
            </div>
        </div>
        <!-- 📋 Tabla de médicos -->
        <div class="mb-3 text-end">
            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalMedico">
                <i class="bi bi-plus-lg"></i> Agregar Médico
            </button>
        </div>

        <asp:GridView ID="gvMedicos" runat="server" CssClass="table table-striped table-bordered"
                      AutoGenerateColumns="False" GridLines="None">
            <Columns>
        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
        <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
        <asp:BoundField DataField="Matricula" HeaderText="Matrícula" />
        <asp:BoundField DataField="Email" HeaderText="Email" />
        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
        <asp:BoundField DataField="DescripcionEspecialidad" HeaderText="Especialidad" />
        
        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-sm btn-outline-secondary me-1">
                    <i class="bi bi-pencil-square"></i>
                </asp:LinkButton>
                <asp:LinkButton ID="btnDesactivar" runat="server" CommandName="Desactivar" CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-sm btn-outline-danger">
                    <i class="bi bi-person-x"></i>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

    </div>
    <!-- 🧾 Modal Alta / Edición -->
    <div class="modal fade" id="modalMedico" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="modalLabel"><i class="bi bi-person-plus"></i> Nuevo Médico</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label>Nombre</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <label>Apellido</label>
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <label>DNI</label>
                        <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <label>Especialidad</label>
                        <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-control" />
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
                        <label>Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Activo" Value="1" />
                            <asp:ListItem Text="Inactivo" Value="0" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
