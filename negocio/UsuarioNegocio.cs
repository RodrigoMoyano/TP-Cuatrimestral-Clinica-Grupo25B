using AccesoDatos;
using Dominio;
using System;
using System.Collections.Generic;

namespace Negocio
{
    public class UsuarioNegocio
    {
        private UsuarioDatos _datos;

        public UsuarioNegocio()
        {
            _datos = new UsuarioDatos();
        }

        public List<Usuario> Listar()
        {
            return _datos.Listar();
        }

        public void Agregar(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario) || string.IsNullOrWhiteSpace(usuario.Contrasenia))
                throw new Exception("Nombre de usuario y contraseña no pueden estar vacíos.");

            if (usuario.Rol == null)
                throw new Exception("Debe asignarse un rol al usuario.");

            _datos.Agregar(usuario);
        }

        public void Modificar(Usuario usuario)
        {
            _datos.Modificar(usuario);
        }

        public void Eliminar(int id)
        {
            _datos.Eliminar(id);
        }

        public bool ValidarLogin(string nombreUsuario, string contrasenia)
        {
            return _datos.ValidarUsuario(nombreUsuario, contrasenia);
        }
    }
}