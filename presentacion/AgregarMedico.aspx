<%@ Page Title="Agregar Médico" Language="C#" MasterPageFile="~/MiMaster.Master"
    AutoEventWireup="true" CodeBehind="AgregarMedico.aspx.cs"
    Inherits="presentacion.AgregarMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2 class="mb-4">Alta de Médico</h2>


    <div class="card mb-4">
        <div class="card-header bg-primary text-white">Datos de Usuario</div>
        <div class="card-body">

            <asp:ValidationSummary ID="vsUsuario" runat="server" CssClass="text-danger mb-3"
                ValidationGroup="Usuario" HeaderText="Errores en datos de usuario:" />
            <asp:Label ID="lblErrorUsuario" runat="server" CssClass="text-danger"></asp:Label>

            <div class="mb-3">
                <label for="txtUsuario" class="form-label">Nombre de Usuario</label>
                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUsuario" runat="server"
                    ControlToValidate="txtUsuario" ErrorMessage="El nombre de usuario es obligatorio"
                    CssClass="text-danger" ValidationGroup="Usuario" />
            </div>

            <div class="mb-3">
                <label for="txtClave" class="form-label">Contraseña</label>
                <asp:TextBox ID="txtClave" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvClave" runat="server"
                    ControlToValidate="txtClave" ErrorMessage="La contraseña es obligatoria"
                    CssClass="text-danger" ValidationGroup="Usuario" />
            </div>



            <asp:Button ID="btnGuardarUsuario" runat="server" Text="Guardar Usuario"
                CssClass="btn btn-primary" OnClick="btnGuardarUsuario_Click"
                ValidationGroup="Usuario" />
            <asp:Button ID="btnCancelarUsuario" runat="server"
                Text="Cancelar"
                CssClass="btn btn-secondary ms-2"
                OnClick="btnCancelarUsuario_Click"
                CausesValidation="false" />

        </div>
    </div>


    <asp:Panel ID="pnlMedico" runat="server" Visible="false">

        <div class="card mb-4">
            <div class="card-header bg-success text-white">Datos del Médico</div>
            <div class="card-body">

                <asp:ValidationSummary
                    ID="vsMedico"
                    runat="server"
                    CssClass="text-danger mb-3"
                    ValidationGroup="Medico"
                    HeaderText="Errores en datos del médico:"
                    ShowSummary="true"
                    ShowMessageBox="false" />
                <asp:Label ID="lblErrorEmail" runat="server" CssClass="text-danger"></asp:Label><br />
                <asp:Label ID="lblErrorMatricula" runat="server" CssClass="text-danger"></asp:Label>

                <div class="mb-3">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                        ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio"
                        CssClass="text-danger" ValidationGroup="Medico" />
                </div>

                <div class="mb-3">
                    <label for="txtApellido" class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                        ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio"
                        CssClass="text-danger" ValidationGroup="Medico" />
                </div>

                <div class="mb-3">
                    <label for="txtMatricula" class="form-label">Matrícula</label>
                    <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMatricula" runat="server"
                        ControlToValidate="txtMatricula" ErrorMessage="La matrícula es obligatoria"
                        CssClass="text-danger" ValidationGroup="Medico" />
                </div>

                <div class="mb-3">
                    <label for="txtTelefono" class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="12"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTelefono" runat="server"
                        ControlToValidate="txtTelefono" ErrorMessage="El teléfono es obligatorio"
                        CssClass="text-danger" ValidationGroup="Medico" />
                    <asp:RegularExpressionValidator
                        ID="revTelefono"
                        runat="server"
                        ControlToValidate="txtTelefono"
                        ErrorMessage="El teléfono debe contener solo números y un máximo de 12 dígitos."
                        CssClass="text-danger"
                        ValidationGroup="Medico"
                        ValidationExpression="^\d{1,12}$" />
                </div>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Email</label>

                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvEmail"
                        runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="El email es obligatorio"
                        CssClass="text-danger"
                        ValidationGroup="Medico" />

                    <asp:RegularExpressionValidator
                        ID="revEmail"
                        runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="Ingrese un email válido"
                        CssClass="text-danger"
                        ValidationGroup="Medico"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
                </div>

                <div class="mb-3">
                    <label for="chkEspecialidades" class="form-label">Especialidades</label>
                    <asp:CheckBoxList ID="chkEspecialidades" runat="server" CssClass="form-select"></asp:CheckBoxList>
                </div>

            </div>
        </div>


        <div class="card mb-4">
            <div class="card-header bg-info text-white">Turnos de trabajo</div>
            <div class="card-body">

                <asp:ValidationSummary ID="vsTurnos" runat="server" CssClass="text-danger mb-3"
                    ValidationGroup="Turno" HeaderText="Errores en turnos:" />

                <div class="mb-3">
                    <label for="ddlDiaSemana" class="form-label">Día de la semana</label>
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

                <div class="mb-3">
                    <label for="ddlHoraInicio" class="form-label">Hora inicio</label>
                    <asp:DropDownList ID="ddlHoraInicio" runat="server" CssClass="form-select">
                        <asp:ListItem Value="">Seleccione una hora</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <label for="ddlHoraFin" class="form-label">Hora fin</label>
                    <asp:DropDownList ID="ddlHoraFin" runat="server" CssClass="form-select">
                        <asp:ListItem Value="">Seleccione una hora</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <asp:Button ID="btnAgregarTurno" runat="server"
                    Text="Agregar turno"
                    CssClass="btn btn-secondary"
                    OnClick="btnAgregarTurno_Click"
                    ValidationGroup="Turno" />

            </div>
        </div>


        <div class="card mb-4">
            <div class="card-header bg-dark text-white">Turnos agregados</div>
            <div class="card-body">
                <asp:GridView
                    ID="gvTurnos"
                    runat="server"
                    CssClass="table table-bordered"
                    AutoGenerateColumns="false"
                    OnRowCommand="gvTurnos_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="DiaSemanaTexto" HeaderText="Día" />
                        <asp:BoundField DataField="HoraInicio" HeaderText="Inicio" />
                        <asp:BoundField DataField="HoraFin" HeaderText="Fin" />

                        <asp:ButtonField
                            Text="Eliminar"
                            CommandName="Eliminar"
                            ButtonType="Button" />
                    </Columns>

                </asp:GridView>
            </div>
        </div>

        <asp:Button ID="btnGuardarMedico" runat="server" Text="Guardar Médico"
            CssClass="btn btn-success" OnClick="btnGuardarMedico_Click"
            ValidationGroup="Medico" />
        <asp:Button ID="btnCancelarMedico" runat="server"
            Text="Cancelar"
            CssClass="btn btn-secondary ms-2"
            OnClick="btnCancelarMedico_Click"
            CausesValidation="false" />

    </asp:Panel>

</asp:Content>
