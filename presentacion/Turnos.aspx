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
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass=" btn btn-primary" />
            </div>
        </div>
    </nav>
    <nav class="navbar bg-body-tertiary">
        <div class="container-fluid">

            <div class="d-flex gap-3" role="search">
                <asp:Label ID="lblfiltro" runat="server" Text="Filtrar"></asp:Label>
                <asp:TextBox ID="txtfiltro" AutoPostBack="true" OnTextChanged="txtfiltro_TextChanged" CssClass="form-control" runat="server">
                </asp:TextBox>
                <div class="mb-3">
                    <asp:CheckBox ID="chkAvanzado" Text="Filtro Avanzado" runat="server" AutoPostBack="true" OnCheckedChanged="chkAvanzado_CheckedChanged"/>
                </div>--%>
            

               <%if (chkAvanzado.Checked)
            {%>
                <div class="row">
                    <div class="col-3">
                        <div class="mb-3">
                            <asp:Label ID="lblCampo" runat="server"></asp:Label>
                            <asp:DropDownList ID="ddlCampo" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged">
                                <asp:ListItem Text="Paciente" />
                                <asp:ListItem Text="Medico" />
                                <asp:ListItem Text="Especialidad" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-3">
                        <div class="mb-3">
                            <asp:Label ID="lblCriterio" runat="server" Text="Criterio"></asp:Label>
                            <asp:DropDownList ID="ddlCriterio" CssClass="form-control" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-3">
                        <div class="mb-3">
                            <asp:Label ID="lblFiltroAvanzado" runat="server" Text="Filtro"></asp:Label>
                            <asp:TextBox ID="txtFiltroAvanzado" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                </div>
           <%  } %>

               <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                    CssClass="btn btn-primary" />
            </div>
        </div>
    </nav>
    <div>
        <asp:GridView ID="dgvVerTurnos" runat="server" AutoGenerateColumns="false" OnRowCommand="dgvVerTurnos_RowCommand" CssClass="table table-striped" Width="100%">
            <Columns>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Hora">
                    <ItemTemplate>
                        <%# Eval("Hora", "{0:hh\\:mm}") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                <asp:BoundField DataField="Medico" HeaderText="Medico" />
                <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                <asp:BoundField DataField="Estado" HeaderText="Estado Turno" />

                <asp:TemplateField HeaderText="Accion">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnCancelarTurno" runat="server" CssClass="btn btn-sm btn-danger" Text="Cancelar" CommandName="Cancelar" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
