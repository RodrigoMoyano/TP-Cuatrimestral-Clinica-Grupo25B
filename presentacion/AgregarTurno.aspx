<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true"
    CodeBehind="AgregarTurno.aspx.cs" Inherits="presentacion.AgregarTurno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-4">

        <h2>Asignar nuevo turno</h2>
        <hr />

        <div class="mb-3">
            <label>Paciente</label>
            <asp:DropDownList ID="ddlPacientes" runat="server" CssClass="form-select"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvPcientes" runat="server" ControlToValidate="ddlPacientes" InitialValue="" ErrorMessage="Seleccion un paciente" ForeColor="Red" />
        </div>

        <div class="mb-3">
            <label>Especialidad</label>
            <asp:DropDownList ID="ddlEspecialidades" runat="server"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlEspecialidades_SelectedIndexChanged"
                CssClass="form-select">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvEspecialidad" runat="server" ControlToValidate="ddlEspecialidades" InitialValue="Seleccione una especialidad" ForeColor="Red" />
        </div>

        <div class="mb-3">
            <label>Médico</label>
            <asp:DropDownList ID="ddlMedicos" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlMedicos_SelectedIndexChanged"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvMedico" runat="server" ControlToValidate="ddlMedicos" InitialValue="" ErrorMessage="Seleccione un medico" ForeColor="Red"/>
        </div>

        <div class="mb-3">
            <label>Fecha</label>
            <asp:Calendar ID="calFecha" runat="server" OnSelectionChanged="calFecha_SelectionChanged" OnDayRender="calFecha_DayRender"></asp:Calendar>
            <asp:CustomValidator ID="cvFecha" runat="server" ErrorMessage="Selessione una fecha" ForeColor="Red" OnServerValidate="cvFecha_ServerValidate"/>        
        </div> 

        <div class="mb-3">
            <label>Hora</label>
            <asp:DropDownList ID="ddlHoras" runat="server" CssClass="form-select"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvHoras" runat="server" ControlToValidate="ddlHoras" InitialValue="" ErrorMessage="Seleccione una hora" ForeColor="Red"/>
        </div>

        <div class="mb-3">
            <label>Observaciones</label>
            <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
            <asp:RegularExpressionValidator ID="revObservaciones" runat="server" ControlToValidate="txtObservaciones" ValidationExpession="^.{0,100}$" ErrorMessage="Maximo 100 caracteres" ForeColor="Red"/>
        </div>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>

        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Turno" CssClass="btn btn-success" OnClick="btnGuardar_Click" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" BackColor="Red" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" />

    </div>

</asp:Content>
