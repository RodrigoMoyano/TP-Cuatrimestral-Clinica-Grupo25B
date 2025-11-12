<%@ Page Title="Especialidades" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="Especialidades.aspx.cs" Inherits="presentacion.Especialidades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
       
        .card {
            border: none;
            border-radius: 15px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.1);
            overflow: hidden;
            transition: transform 0.2s ease, box-shadow 0.2s ease;
            text-align: center;
            background-color: #ffffff;
        }

        .card:hover {
            transform: translateY(-5px);
            box-shadow: 0 6px 16px rgba(0,0,0,0.15);
        }

        .card-img-top {
            width: 100%;
            height: 220px; 
            object-fit: cover; 
            border-bottom: 2px solid #e9ecef;
        }

        .card-body {
            padding: 1rem;
        }

        .card-title {
            font-size: 1.1rem;
            font-weight: 600;
            color: #007b8f;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h2 class="text-center text-primary mb-4">Especialidades de la Clínica</h2>

        <div class="row row-cols-1 row-cols-md-2 g-4">
            <asp:Repeater ID="repEspecialidades" runat="server">
                <ItemTemplate>
                    <div class="col">
                        <div class="card h-100 card-especialidad">
                            <img src='<%# GetImagenPorEspecialidad(Eval("Descripcion").ToString()) %>' 
                                 class="card-img-top" alt='<%# Eval("Descripcion") %>' />
                            <div class="card-body">
                                <h5 class="card-title"><%# Eval("Descripcion") %></h5>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>