using Dominio;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Negocio
{
    public class UsuarioNegocio
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                        SELECT u.Id, u.NombreUsuario, u.Clave, u.Activo,
                               r.Id AS RolId, r.Descripcion AS RolDescripcion
                        FROM Usuario u
                        INNER JOIN Rol r 
                        ON u.IdRol = r.Id
                    ");
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Usuario u = new Usuario
                        {
                            Id = (int)datos.Lector["Id"],
                            NombreUsuario = datos.Lector["NombreUsuario"].ToString(),
                            Clave = datos.Lector["Clave"].ToString(),
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
                    datos.SetearConsulta("INSERT INTO Usuario (NombreUsuario, Clave, Activo, IdRol) VALUES (@NombreUsuario, @Clave, @Activo, @IdRol)");
                    datos.SetearParametro("@NombreUsuario", usuario.NombreUsuario);
                    datos.SetearParametro("@Clave", usuario.Clave);
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
        public bool Login(Usuario usuario)
        {
            Datos datos = new Datos();

            try
            {
                datos.SetearConsulta(@"SELECT u.Id, u.NombreUsuario, u.Clave, r.Id AS RolId, r.Descripcion AS RolDescripcion FROM Usuario u INNER JOIN Rol r ON u.IdRol = r.Id WHERE u.NombreUsuario = @NombreUsuario AND u.Clave = @Clave AND u.Activo = 1");
                datos.SetearParametro("@NombreUsuario", usuario.NombreUsuario);
                datos.SetearParametro("@Clave", usuario.Clave);
                
                datos.EjecutarLectura();
                
                if (datos.Lector.Read())
                {
                    usuario.Id = (int)datos.Lector["Id"];
                    usuario.Rol = new Rol();
                    usuario.Rol.Id = (int)datos.Lector["RolId"];
                    usuario.Rol.Descripcion = (string)datos.Lector["RolDescripcion"];

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Modificar(Usuario usuario)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("UPDATE Usuario SET NombreUsuario=@NombreUsuario, Clave=@Clave, Activo=@Activo, IdRol=@IdRol WHERE Id=@Id");
                    datos.SetearParametro("@NombreUsuario", usuario.NombreUsuario);
                    datos.SetearParametro("@Clave", usuario.Clave);
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

        public bool ValidarUsuario(string nombreUsuario, string clave)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("SELECT COUNT(*) FROM Usuario WHERE NombreUsuario=@NombreUsuario AND Clave=@Clave AND Activo=1");
                    datos.SetearParametro("@NombreUsuario", nombreUsuario);
                    datos.SetearParametro("@Clave", clave);

                    int count = datos.EjecutarAccionEscalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al validar usuario: " + ex.Message);
                }
            }
        }
        public Usuario ObtenerUsuario(string nombreUsuario, string clave)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                        SELECT u.Id, u.NombreUsuario, u.Clave, u.Activo,
                               r.Id AS RolId, r.Descripcion AS RolDescripcion
                        FROM Usuario u
                        INNER JOIN Rol r ON u.IdRol = r.Id
                        WHERE u.NombreUsuario = @NombreUsuario AND u.Clave = @Clave AND u.Activo = 1
            ");
                    datos.SetearParametro("@NombreUsuario", nombreUsuario);
                    datos.SetearParametro("@Clave", clave);
                    datos.EjecutarLectura();

                    if (datos.Lector.Read())
                    {
                        return new Usuario
                        {
                            Id = (int)datos.Lector["Id"],
                            NombreUsuario = datos.Lector["NombreUsuario"].ToString(),
                            Clave = datos.Lector["Clave"].ToString(),
                            Activo = (bool)datos.Lector["Activo"],
                            Rol = new Rol
                            {
                                Id = (int)datos.Lector["RolId"],
                                Descripcion = datos.Lector["RolDescripcion"].ToString()
                            }
                        };
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener usuario: " + ex.Message);
                }
            }
        }
        //Verificamos si el correo ya existe
        public bool ExisteUsuario(string nombreUsuario)
        {
            using(Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("Select Count(*) From Usuario Where NombreUsuario = @NombreUsuario");
                    datos.SetearParametro("@NombreUsuario", nombreUsuario);

                    int count = datos.EjecutarAccionEscalar();

                    return count > 0;
                }
                catch (Exception ex)
                {

                    throw new Exception("Usuario Existente.", ex);
                }
            }
        }

        public int AgregarYObtenerId(Usuario usuario)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                INSERT INTO Usuario (NombreUsuario, Clave, Activo, IdRol)
                OUTPUT INSERTED.Id
                VALUES (@NombreUsuario, @Clave, @Activo, @IdRol)");

                    datos.SetearParametro("@NombreUsuario", usuario.NombreUsuario);
                    datos.SetearParametro("@Clave", usuario.Clave);
                    datos.SetearParametro("@Activo", usuario.Activo);
                    datos.SetearParametro("@IdRol", usuario.Rol.Id);

                    int idGenerado = datos.EjecutarAccionEscalar(); // ✅ devuelve el identity
                    usuario.Id = idGenerado;
                    return idGenerado;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar usuario: " + ex.Message);
                }
            }
        }


    }
}