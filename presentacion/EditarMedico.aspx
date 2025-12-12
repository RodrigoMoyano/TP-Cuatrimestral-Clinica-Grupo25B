<%@ Page Title="Editar Médico" Language="C#" MasterPageFile="~/MiMaster.Master"
    AutoEventWireup="true" CodeBehind="EditarMedico.aspx.cs"
    Inherits="presentacion.EditarMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3 class="mb-4">Editar Médico</h3>

    <asp:ValidationSummary 
    ID="vsEditarMedico"
    runat="server"
    CssClass="text-danger mb-3"
    HeaderText="Corrija los siguientes errores:"
    DisplayMode="BulletList" />

    <asp:HiddenField ID="hfIdMedico" runat="server" />

    <div class="card p-4 shadow-sm">

       
        <div class="mb-3">
            <label>Nombre</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
        </div>

       
        <div class="mb-3">
            <label>Apellido</label>
            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
        </div>

    
        <div class="mb-3">
            <label>Matrícula</label>
            <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" />
        </div>

       
       <div class="mb-3">
    <label>Teléfono</label>

    <asp:TextBox 
        ID="txtTelefono" 
        runat="server" 
        CssClass="form-control" 
        MaxLength="12" />

    <!-- Solo números -->
    <asp:RegularExpressionValidator
        ID="revTelefonoNumeros"
        runat="server"
        ControlToValidate="txtTelefono"
        ValidationExpression="^\d+$"
        ErrorMessage="El teléfono solo puede contener números."
        CssClass="text-danger"
        Display="Dynamic" />

    <!-- Máximo 12 dígitos -->
    <asp:RegularExpressionValidator
        ID="revTelefonoLongitud"
        runat="server"
        ControlToValidate="txtTelefono"
        ValidationExpression="^\d{1,12}$"
        ErrorMessage="El teléfono no puede tener más de 12 dígitos."
        CssClass="text-danger"
        Display="Dynamic" />
</div>

        
        <div class="mb-3">
            <label>Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
        </div>

        
        <div class="mb-3">
            <label>Especialidades</label>
            <asp:CheckBoxList ID="chkEspecialidades" runat="server" CssClass="form-select"></asp:CheckBoxList>
        </div>

    </div>

    <br />

  

    <div class="card p-4 shadow-sm mb-4">

        <h5 class="mb-3">Disponibilidad laboral</h5>

        <div class="row">
            <div class="col-md-4">
                <label>Día</label>
                <asp:DropDownList ID="ddlDiaSemana" runat="server" CssClass="form-select">
                    <asp:ListItem Value="1">Lunes</asp:ListItem>
                    <asp:ListItem Value="2">Martes</asp:ListItem>
                    <asp:ListItem Value="3">Miércoles</asp:ListItem>
                    <asp:ListItem Value="4">Jueves</asp:ListItem>
                    <asp:ListItem Value="5">Viernes</asp:ListItem>
                    <asp:ListItem Value="6">Sábado</asp:ListItem>
                    <asp:ListItem Value="0">Domingo</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="col-md-4">
                <label>Hora inicio</label>
                <asp:DropDownList ID="ddlHoraInicio" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>

            <div class="col-md-4">
                <label>Hora fin</label>
                <asp:DropDownList ID="ddlHoraFin" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
        </div>

        <asp:Button ID="btnAgregarTurno" runat="server"
            Text="Agregar Turno"
            CssClass="btn btn-success mt-3"
            OnClick="btnAgregarTurno_Click" />

    </div>

    
    <asp:GridView ID="gvTurnos" runat="server" AutoGenerateColumns="False"
        CssClass="table table-bordered"
        OnRowCommand="gvTurnos_RowCommand"
        DataKeyNames="Id">

        <Columns>
            <asp:BoundField DataField="DiaSemanaTexto" HeaderText="Día" />
            <asp:BoundField DataField="HoraInicio" HeaderText="Inicio" />
            <asp:BoundField DataField="HoraFin" HeaderText="Fin" />

            <asp:ButtonField Text="Eliminar" CommandName="Eliminar" ButtonType="Button" />
        </Columns>

    </asp:GridView>

    <br />

   
    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios"
        CssClass="btn btn-primary me-2"
        OnClick="btnGuardar_Click" />

    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
        CssClass="btn btn-secondary"
        OnClick="btnCancelar_Click" />

</asp:Content>