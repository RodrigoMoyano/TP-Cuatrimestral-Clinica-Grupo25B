using AccesoDatos;
using Dominio;
using System;
using System.Collections.Generic;

namespace Negocio
{
    public class EstadoTurnoNegocio
    {
        private EstadoTurnoDatos _datos;

        public EstadoTurnoNegocio()
        {
            _datos = new EstadoTurnoDatos();
        }

        public List<EstadoTurno> Listar()
        {
            return _datos.Listar();
        }

        public void Agregar(EstadoTurno estado)
        {
            if (string.IsNullOrWhiteSpace(estado.Descripcion))
                throw new Exception("La descripción del estado no puede estar vacía.");

            _datos.Agregar(estado);
        }

        public void Modificar(EstadoTurno estado)
        {
            _datos.Modificar(estado);
        }

        public void Eliminar(int id)
        {
            _datos.Eliminar(id);
        }
    }
}