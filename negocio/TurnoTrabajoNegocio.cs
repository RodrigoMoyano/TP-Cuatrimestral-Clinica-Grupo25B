using AccesoDatos;
using Dominio;
using System;
using System.Collections.Generic;

namespace Negocio
{
    public class TurnoTrabajoNegocio
    {
        private TurnoTrabajoDatos _datos;

        public TurnoTrabajoNegocio()
        {
            _datos = new TurnoTrabajoDatos();
        }

        public List<TurnoTrabajo> Listar()
        {
            return _datos.Listar();
        }

        public void Agregar(TurnoTrabajo turno)
        {
            if (turno.HoraInicio >= turno.HoraFin)
                throw new Exception("La hora de inicio debe ser menor que la hora de fin.");

            if (turno.Medico == null)
                throw new Exception("Debe asignarse un médico al turno de trabajo.");

            _datos.Agregar(turno);
        }

        public void Modificar(TurnoTrabajo turno)
        {
            _datos.Modificar(turno);
        }

        public void Eliminar(int id)
        {
            _datos.Eliminar(id);
        }
    }
}