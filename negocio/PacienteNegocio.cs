using Dominio;
using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PacienteNegocio
    {
        private readonly PacienteDatos _pacienteDatos;

        public PacienteNegocio()
        {
            _pacienteDatos = new PacienteDatos();
        }

        public List<Paciente> ListarPacientes()
        {
            return _pacienteDatos.ObtenerTodos();
        }

        public void AgregarPaciente(Paciente paciente)
        {
            
            if (string.IsNullOrEmpty(paciente.Nombre) || string.IsNullOrEmpty(paciente.Apellido))
                throw new System.Exception("El nombre y apellido son obligatorios.");

            
            _pacienteDatos.Agregar(paciente);
        }

        public void ModificarPaciente(Paciente paciente)
        {
            
            _pacienteDatos.Modificar(paciente);
        }

        public void EliminarPaciente(int idPaciente)
        {
            
            _pacienteDatos.Eliminar(idPaciente);
        }
    }
}

