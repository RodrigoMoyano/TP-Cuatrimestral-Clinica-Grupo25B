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

            <div class="d-flex gap-3" role="search">
                <asp:Label ID="lblfiltro" runat="server" Text="Filtrar"></asp:Label>
                <asp:TextBox ID="txtfiltro" AutoPostBack="true" OnTextChanged="txtfiltro_TextChanged" CssClass="form-control" runat="server">
                </asp:TextBox>
            </div>
        </div>
    </nav>
    <div class="container mt-4">
        <asp:GridView ID="dgvVerTurnos" runat="server"
            AutoGenerateColumns="false"
            CssClass="table table-striped table-hover"
            Width="100%"
            DataKeyNames="IdTurno"
            OnRowCommand="dgvVerTurnos_RowCommand">

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
                        <asp:LinkButton ID="btnReprogramar" runat="server"
                            CssClass="btn btn-sm btn-warning"
                            Text="Reprogramar"
                            CausesValidation="false"
                            CommandName="Reprogramar"
                            CommandArgument='<%# Eval("IdTurno") %>'> 
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>