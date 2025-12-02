<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="TurnoPaciente.aspx.cs" Inherits="presentacion.TurnoPaciente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="mb-3">
            <asp:DropDownList ID="ddlFiltroEstado" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="ddlFiltroEstado_SelectedIndexChanged"
                CssClass="form-select w-25 mb-3">
                <asp:ListItem Value="">Todos</asp:ListItem>
                <asp:ListItem Value="Nuevo">Nuevo</asp:ListItem>
                <asp:ListItem Value="Reprogramado">Reprogramado</asp:ListItem>
                <asp:ListItem Value="Cancelado">Cancelado</asp:ListItem>
                <asp:ListItem Value="No Asistió">No Asistió</asp:ListItem>
                <asp:ListItem Value="Cerrado">Cerrado  </asp:ListItem>
            </asp:DropDownList>
        </div>
            <asp:GridView 
            ID="gvTurnos" 
            runat="server" 
            AutoGenerateColumns="False" 
            CssClass="table"
            OnRowCommand="gvTurnos_RowCommand"
            OnRowDataBound="gvTurnos_RowDataBound">

            <Columns>

                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Hora" HeaderText="Hora" />

                <asp:BoundField DataField="Especialidad.Descripcion" HeaderText="Especialidad" />

                <asp:TemplateField HeaderText="Médico">
                    <ItemTemplate>
                        <%# Eval("Medico.Apellido") + " " + Eval("Medico.Nombre") %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Estado.Descripcion" HeaderText="Estado" />

                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:Button ID="btnCancelar"
                                    runat="server"
                                    Text="Cancelar"
                                    CommandName="Cancelar"
                                    CommandArgument='<%# Eval("Id") %>'
                                    CssClass="btn btn-danger btn-sm" />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
</asp:Content>
