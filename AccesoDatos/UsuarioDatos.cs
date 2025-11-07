using System;
using System.Collections.Generic;
using Dominio;

namespace AccesoDatos
{
    public class UsuarioDatos
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                        SELECT u.Id, u.NombreUsuario, u.Contrasenia, u.Activo,
                               r.Id AS RolId, r.Nombre AS RolNombre
                        FROM Usuario u
                        INNER JOIN Rol r ON u.IdRol = r.Id
                    ");
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Usuario u = new Usuario
                        {
                            Id = (int)datos.Lector["Id"],
                            NombreUsuario = datos.Lector["NombreUsuario"].ToString(),
                            Contrasenia = datos.Lector["Contrasenia"].ToString(),
                            Activo = (bool)datos.Lector["Activo"],
                            Rol = new Rol
                            {
                                Id = (int)datos.Lector["RolId"],
                                Descripcion = datos.Lector["RolDescripcion"].ToString()
                            }
                        };
                        lista.Add(u);
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar usuarios: " + ex.Message);
                }
            }
        }

        public void Agregar(Usuario usuario)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("INSERT INTO Usuario (NombreUsuario, Contrasenia, Activo, IdRol) VALUES (@NombreUsuario, @Contrasenia, @Activo, @IdRol)");
                    datos.SetearParametro("@NombreUsuario", usuario.NombreUsuario);
                    datos.SetearParametro("@Contrasenia", usuario.Contrasenia);
                    datos.SetearParametro("@Activo", usuario.Activo);
                    datos.SetearParametro("@IdRol", usuario.Rol.Id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar usuario: " + ex.Message);
                }
            }
        }

        public void Modificar(Usuario usuario)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("UPDATE Usuario SET NombreUsuario=@NombreUsuario, Contrasenia=@Contrasenia, Activo=@Activo, IdRol=@IdRol WHERE Id=@Id");
                    datos.SetearParametro("@NombreUsuario", usuario.NombreUsuario);
                    datos.SetearParametro("@Contrasenia", usuario.Contrasenia);
                    datos.SetearParametro("@Activo", usuario.Activo);
                    datos.SetearParametro("@IdRol", usuario.Rol.Id);
                    datos.SetearParametro("@Id", usuario.Id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar usuario: " + ex.Message);
                }
            }
        }

        public void Eliminar(int id)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("DELETE FROM Usuario WHERE Id=@Id");
                    datos.SetearParametro("@Id", id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar usuario: " + ex.Message);
                }
            }
        }

        public bool ValidarUsuario(string nombreUsuario, string contrasenia)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("SELECT COUNT(*) FROM Usuario WHERE NombreUsuario=@NombreUsuario AND Contrasenia=@Contrasenia AND Activo=1");
                    datos.SetearParametro("@NombreUsuario", nombreUsuario);
                    datos.SetearParametro("@Contrasenia", contrasenia);

                    int count = datos.EjecutarAccionEscalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al validar usuario: " + ex.Message);
                }
            }
        }
    }
}