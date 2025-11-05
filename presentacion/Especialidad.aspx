<%@ Page Title="Especialidad" Language="C#" MasterPageFile="~/MiMaster.master" AutoEventWireup="true" CodeBehind="Especialidad.aspx.cs" Inherits="presentacion.Especialidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Especialidades</h2>
    <asp:GridView ID="gvEspecialidad" runat="server" CssClass="table table-striped"></asp:GridView>
</asp:Content>
