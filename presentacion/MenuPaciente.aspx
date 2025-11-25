<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="MenuPaciente.aspx.cs" Inherits="presentacion.MenuPaciente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        html, body {
            height: 100%;
        }

        .main-container {
            min-height: calc(100vh - 80px);
            display: flex;
            flex-direction: column;
            justify-content: flex-start;
        }

        footer {
            margin-top: auto;
        }

        .perfil-paciente {
            width: 180px;
            height: 180px;
            object-fit: cover;
            border-radius: 50%;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }

        .card {
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        }

        .card:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 16px rgba(0,0,0,0.15);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container main-container mt-4">
        <div class="text-center my-4">
            <img src="https://tse1.mm.bing.net/th/id/OIP.SOjz5zcy2maczlZBcCwCegHaE8?cb=ucfimg2ucfimg=1&rs=1&pid=ImgDetMain&o=7&rm=3"
                 alt="Paciente" class="perfil-paciente mb-3" />
            <h2 class="fw-bold text-primary">Bienvenido/a a tu espacio personal</h2>
            <p class="text-muted">Gestioná tus turnos, descubrí especialidades y accedé a toda tu información médica.</p>
        </div>

        <div class="row text-center mt-5 justify-content-center">
            <div class="col-md-3 mb-4">
                <div class="card p-3 shadow-sm border-0">
                    <i class="bi bi-calendar-check text-success fs-1"></i>
                    <h5 class="mt-3">Ver mis turnos</h5>
                    <asp:Button ID="btnVerTurnos" runat="server" CssClass="btn btn-outline-primary mt-2" Text="Ingresar" OnClick="btnVerTurnos_Click" />
                </div>
            </div>

            <div class="col-md-3 mb-4">
                <div class="card p-3 shadow-sm border-0">
                    <i class="bi bi-plus-circle text-primary fs-1"></i>
                    <h5 class="mt-3">Pedir nuevo turno</h5>
                    <asp:Button ID="btnPedirTurno" runat="server" CssClass="btn btn-outline-success mt-2" Text="Ingresar" OnClick="btnPedirTurno_Click" />
                </div>
            </div>

            
        </div>
    </div>

    <footer class="navbar navbar-expand-lg navbar-dark bg-dark py-3">
        <div class="container justify-content-center text-center">
            <span class="navbar-text text-light">
                📍 Clínica Médica - Av. Siempre Viva 742, Buenos Aires &nbsp; | &nbsp;
                ☎️ (011) 4567-8910 &nbsp; | &nbsp;
                ✉️ contacto@clinicamedica.com
            </span>
        </div>
    </footer>

</asp:Content>