using AccesoDatos;
using Dominio;
using System;
using System.Collections.Generic;

namespace Negocio
{
    public class RolNegocio
    {
        private RolDatos _datos;

        public RolNegocio()
        {
            _datos = new RolDatos();
        }

        public List<Rol> Listar()
        {
            return _datos.Listar();
        }

        public void Agregar(Rol rol)
        {
            if (string.IsNullOrWhiteSpace(rol.Descripcion))
                throw new Exception("La descripción del rol no puede estar vacía.");

            _datos.Agregar(rol);
        }

        public void Modificar(Rol rol)
        {
            _datos.Modificar(rol);
        }

        public void Eliminar(int id)
        {
            _datos.Eliminar(id);
        }
    }
}