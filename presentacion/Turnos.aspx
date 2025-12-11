<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="Turnos.aspx.cs" Inherits="presentacion.Turnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Gestion de Turnos</h1>
    <nav class="navbar navbar-expand-lg bg-body-tertiary">
        <div class="container-fluid">
            <div class="collapse navbar-collapse" id="navbarScroll">
                <ul class="navbar-nav me-auto my-2 my-lg-0 navbar-nav-scroll" style="--bs-scroll-height: 100px;">

                    <li class="nav-item dropdown">
                        <ul class="dropdown-menu">
                            <li><a class="dropdown-item" href="#">Action</a></li>
                            <li><a class="dropdown-item" href="#">Another action</a></li>
                            <li>
                                <hr class="dropdown-divider">
                            </li>
                            <li><a class="dropdown-item" href="#">Something else here</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    <nav class="navbar bg-body-tertiary">
    <div class="container-fluid">

        <div class="d-flex justify-content-between align-items-center w-100">

            <div class="d-flex align-items-center gap-4">

                <!--Filtro Paciente -->
                <div class="d-flex align-items-center gap-2">
                    <asp:Label ID="lblfiltro" runat="server" Text="Paciente:"></asp:Label>
                    <asp:TextBox ID="txtfiltro" runat="server"
                        AutoPostBack="true"
                        CssClass="form-control"
                        OnTextChanged="txtfiltro_TextChanged"
                        Style="width: 200px;">
                    </asp:TextBox>
                </div>

                <!--Filtro Estado -->
                <div class="d-flex align-items-center gap-2">
                    <asp:Label ID="lblFitroEstado" runat="server" Text="Estado:"></asp:Label>
                    <asp:DropDownList ID="ddlFiltroEstado" runat="server"
                        AutoPostBack="true"
                        CssClass="form-select"
                        OnSelectedIndexChanged="ddlFiltroEstado_SelectedIndexChanged"
                        Style="width: 200px;">
                        <asp:ListItem Value="">Todos</asp:ListItem>
                        <asp:ListItem Value="Nuevo">Nuevo</asp:ListItem>
                        <asp:ListItem Value="Reprogramado">Reprogramado</asp:ListItem>
                        <asp:ListItem Value="Cancelado">Cancelado</asp:ListItem>
                        <asp:ListItem Value="No Asistió">No Asistió</asp:ListItem>
                        <asp:ListItem Value="Cerrado">Cerrado</asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>

            <div>
                <asp:Button ID="btnAgregarTurno" runat="server"
                    Text="Agregar Turno"
                    CssClass="btn btn-primary"
                    OnClick="btnAgregarTurno_Click" />
            </div>

        </div>

    </div>
</nav>
    <!--DGV-->
    <div class="mb-3">
        <asp:GridView ID="dgvVerTurnos" runat="server"
            AutoGenerateColumns="false"
            CssClass="table table-striped table-hover"
            Width="100%"
            DataKeyNames="IdTurno"
            AllowPaging ="true"
            PageSize="10"
            PagerSettings-Mode="Numeric"
            PagerStyle-HorizontalAlign="Center"
            PagerStyle-CssClass="pagination-grid"
            OnPageIndexChanging="dgvVerTurnos_PageIndexChanging"
            OnRowCommand="dgvVerTurnos_RowCommand"
            OnRowDataBound="dgvVerTurnos_RowDataBound">

            <Columns>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />

                <asp:TemplateField HeaderText="Hora">
                    <ItemTemplate>
                        <%# Eval("Hora", "{0:hh\\:mm}") %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                <asp:BoundField DataField="Medico" HeaderText="Médico" />
                <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                <asp:BoundField DataField="Estado" HeaderText="Estado" />

                <asp:TemplateField HeaderText="Acción">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnReprogramar" runat="server" CssClass="btn btn-sm btn-warning" Text="Reprogramar" CausesValidation="false" CommandName="Reprogramar" CommandArgument='<%# Eval("IdTurno") %>'></asp:LinkButton>
                        <asp:LinkButton ID="btnCancelar" runat="server" Text="Cancelar" CommandName="Cancelar" CommandArgument='<%#Eval("IdTurno")%>' CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Seguro que desea cancelar este turno?');"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
