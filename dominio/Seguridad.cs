using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Dominio
{
    public static class Seguridad
    {
        //Creo constantes para compararlas con el idrol que se recibio
        private const int ID_Administrador = 1;
        private const int ID_Medico = 2;
        private const int ID_Paciente = 3;

        //Chequeo que se haya iniciado sesion para poder navegar entre paginas, si no que obligue a loguearse
        public static bool sessionActiva(object user)
        {
            Usuario usuario = user != null ? (Usuario)user : null;
            if (usuario != null && usuario.Id != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Capturo el valor del Rol del usuario
        public static Rol obtenerRol(object user)
        {
            Usuario usuario = user as Usuario;

            return usuario  != null ? usuario.Rol : null;
        }
        /*public static bool esAdmin(object user)
        {
            Rol rol = obtenerRol(user);

            return rol != null && rol.Id == ID_Administrador;
        }
        public static bool esMedico(object user)
        {
            Rol rol = obtenerRol(user);

            return rol != null && rol.Id == ID_Medico;
        }*/

        /*public static bool esPaciente(object user)
        {
            Rol rol = obtenerRol(user);

            return rol != null & rol.Id == ID_Paciente;
        }*/
        public static bool esPaciente(object user)
        {
            Usuario usuario = (Usuario)user;
            return usuario.Rol.Descripcion == "Paciente";
        }
        public static bool esMedico(object user)
        {
            Usuario usuario = (Usuario)user;
            return usuario.Rol.Descripcion == "Medico";
        }
        public static bool esAdmin(object user)
        {
            Usuario usuario = (Usuario)user;
            return usuario.Rol.Descripcion == "Admin";
        }
    }

}
