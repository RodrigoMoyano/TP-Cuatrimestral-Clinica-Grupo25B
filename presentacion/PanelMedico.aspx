<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PanelMedico.aspx.cs"
    MasterPageFile="~/MiMaster.Master"
    Inherits="Presentacion.PanelMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- TÍTULO PRINCIPAL -->
    <h3 class="text-center mb-4" style="font-size: 32px; font-weight: bold;">Panel del Médico
    </h3>

    <!-- CARD INFORMACIÓN DEL MÉDICO -->
    <div class="card shadow-sm mb-4">
        <div class="card-header bg-primary text-white">
            Información del Médico
        </div>

        <div class="card-body">

            <p class="mb-2">
                <strong>Nombre: </strong>
                <asp:Label ID="lblNombreMedico" runat="server"></asp:Label>
            </p>

            <p class="mb-2">
                <strong>Especialidades: </strong>
                <asp:Label ID="lblEspecialidades" runat="server"></asp:Label>
            </p>

        </div>
    </div>

    <!-- CARD TURNOS -->
    <div class="card shadow-sm mb-4">
        <div class="card-header bg-primary text-white">
            Turnos Asignados
        </div>

        <div class="card-body">
            <!-- 🔍 FILTROS -->
<div class="row mb-3">

    <!-- Filtro por paciente -->
    <div class="col-md-4 mb-2">
        <label><strong>Paciente:</strong></label>
        <asp:TextBox ID="txtBuscarPaciente" runat="server"
            CssClass="form-control"
            AutoPostBack="true"
            OnTextChanged="txtBuscarPaciente_TextChanged" />
    </div>

    <!-- Filtro por estado del turno -->
    <div class="col-md-4 mb-2">
        <label><strong>Estado del turno:</strong></label>
        <asp:DropDownList ID="ddlEstado" runat="server"
            CssClass="form-select"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">

            <asp:ListItem Value="0">Todos</asp:ListItem>
            <asp:ListItem Value="Nuevo">Nuevo</asp:ListItem>
            <asp:ListItem Value="Reprogramado">Reprogramado</asp:ListItem>
            <asp:ListItem Value="Cancelado">Cancelado</asp:ListItem>
            <asp:ListItem Value="No Asistió">No Asistió</asp:ListItem>
            <asp:ListItem Value="Cerrado">Cerrado</asp:ListItem>

        </asp:DropDownList>
    </div>

    <!-- Ordenar por fecha -->
    <div class="col-md-4 mb-2">
        <label><strong>Ordenar por fecha:</strong></label>
        <asp:DropDownList ID="ddlOrdenFecha" runat="server"
            CssClass="form-select"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlOrdenFecha_SelectedIndexChanged">
            <asp:ListItem Value="desc">Más recientes primero</asp:ListItem>
            <asp:ListItem Value="asc">Más antiguos primero</asp:ListItem>
        </asp:DropDownList>
    </div>

</div>



            <asp:GridView
                ID="gvTurnos"
                runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="Id"
                OnRowCommand="gvTurnos_RowCommand"
                CssClass="table table-striped table-bordered">

                <Columns>

                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />

                    <asp:TemplateField HeaderText="Hora">
                        <ItemTemplate>
                            <%# FormatearHora(Eval("Hora")) %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Paciente">
                        <ItemTemplate>
                            <%# Eval("Paciente.Nombre") + " " + Eval("Paciente.Apellido") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <%# Eval("Estado.Descripcion") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:ButtonField
                        Text="Ver Observaciones"
                        CommandName="VerObservaciones"
                        ButtonType="Button" />

                </Columns>

            </asp:GridView>

        </div>
    </div>

</asp:Content>
