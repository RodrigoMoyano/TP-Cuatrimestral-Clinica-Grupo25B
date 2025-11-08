<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="Turnos.aspx.cs" Inherits="presentacion.Turno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Gestion de Turnos</h1>
    <nav class="navbar navbar-expand-lg bg-body-tertiary">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">TURNOS</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarScroll" aria-controls="navbarScroll" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarScroll">
                <ul class="navbar-nav me-auto my-2 my-lg-0 navbar-nav-scroll" style="--bs-scroll-height: 100px;">

                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">Link
                        </a>
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
                <a class="btn btn-primary" href="#" role="button">+ Agregar</a>

            </div>
        </div>
    </nav>
    <nav class="navbar bg-body-tertiary">
        <div class="container-fluid">
            <form class="d-flex" role="search">
                <input class="form-control me-2" type="search" placeholder="Search" aria-label="Search" />
                <button class="btn btn-outline-success" type="submit">Search</button>
            </form>
        </div>
    </nav>
    <div>
        <asp:GridView ID="dgvVerTurnos" runat="server" AutoGenerateColumns="false" CssClass="table table-striped" Width="100%">
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
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
