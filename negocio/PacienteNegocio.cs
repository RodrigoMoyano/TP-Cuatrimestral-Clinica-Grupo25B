using Dominio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Negocio
{
    public class PacienteNegocio
    {

        public void RegistrarPaciente(Paciente nuevo)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@" Insert Into Usuario (NombreUsuario, Clave, IdRol, Activo) Values(@NombreUsuario, @Clave, 2, 1) Select Scope_Identity()");

                    datos.SetearParametro("@NombreUsuario", nuevo.Usuario.NombreUsuario);
                    datos.SetearParametro("@Clave", nuevo.Usuario.Clave);

                    int IdUsuarioCreado = datos.EjecutarAccionEscalar();

                    datos.SetearConsulta(@"Insert Into Paciente (Nombre, Apellido, Dni, Email, Telefono, IdUsuario, IdCobertura)
                                        Values (@Nombre, @Apellido, @Dni, @EmailPaciente, @Telefono, @IdUsuario, @IdCobertura)");

                    datos.SetearParametro("@Nombre", nuevo.Nombre);
                    datos.SetearParametro("@Apellido", nuevo.Apellido);
                    datos.SetearParametro("@Dni", nuevo.Dni);
                    datos.SetearParametro("@EmailPaciente", nuevo.Email);
                    datos.SetearParametro("@Telefono", nuevo.Telefono);

                    datos.SetearParametro("@IdUsuario", IdUsuarioCreado);

                    datos.SetearParametro("@IdCobertura", nuevo.Cobertura.Id);

                    datos.EjecutarAccion();
                    
                }
                catch (Exception ex)
                {
                    
                    throw new Exception("Error al registrar paciente: " + ex.Message, ex);
                }
            }
        }
        public List<Paciente> ObtenerTodos()
        {
            List<Paciente> lista = new List<Paciente>();

            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("SELECT  P.Id, P.Nombre, P.Apellido, P.Dni, P.Email, P.Telefono,  P.IdUsuario, U.NombreUsuario, U.Activo AS ActivoUsuario FROM Paciente P LEFT JOIN Usuario U ON P.IdUsuario = U.Id");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Paciente p = new Paciente
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Dni = datos.Lector["Dni"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Telefono = datos.Lector["Telefono"].ToString(),
                        
                    };
                    if (datos.Lector["IdUsuario"] != DBNull.Value)
                    {
                        p.Usuario = new Usuario
                        {
                            Id = (int)datos.Lector["IdUsuario"],
                            NombreUsuario = datos.Lector["NombreUsuario"].ToString(),
                            Activo = datos.Lector["ActivoUsuario"] != DBNull.Value
                            && (bool)datos.Lector["ActivoUsuario"]
                        };
                    }

                    lista.Add(p);
                }
            }

            return lista;
        }

        public void Agregar(Paciente paciente)
        {
            using (Datos datos = new Datos())
            {
                
                datos.SetearConsulta("INSERT INTO Paciente (Nombre, Apellido, Dni, FechaNacimiento, Email, Telefono) " +
                                     "VALUES (@Nombre, @Apellido, @Dni, @FechaNacimiento, @Email, @Telefono)");

                datos.SetearParametro("@Nombre", paciente.Nombre);
                datos.SetearParametro("@Apellido", paciente.Apellido);
                datos.SetearParametro("@Dni", paciente.Dni);
                datos.SetearParametro("@FechaNacimiento", paciente.FechaNacimiento);
                datos.SetearParametro("@Email", paciente.Email);
                datos.SetearParametro("@Telefono", paciente.Telefono);

                datos.EjecutarAccion();
            }
        }

        public void Modificar(Paciente paciente)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("UPDATE Paciente SET Nombre = @Nombre, Apellido = @Apellido, Dni = @Dni, " +
                                     "Email = @Email, Telefono = @Telefono, IdUsuario = @IdUsuario " +
                                     "WHERE Id = @Id");

                datos.SetearParametro("@Nombre", paciente.Nombre);
                datos.SetearParametro("@Apellido", paciente.Apellido);
                datos.SetearParametro("@Dni", paciente.Dni);
                datos.SetearParametro("@Email", paciente.Email);
                datos.SetearParametro("@Telefono", paciente.Telefono);
                datos.SetearParametro("@IdUsuario", paciente.Usuario.Id);
                datos.SetearParametro("@IdCobertura", paciente.Cobertura?.Id);
                datos.SetearParametro("@Id", paciente.Id);

                datos.EjecutarAccion();
            }
        }

        public void Eliminar(int id)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("DELETE FROM Paciente WHERE Id = @Id");
                datos.SetearParametro("@Id", id);
                datos.EjecutarAccion();
            }
        }

        public int ObtenerIdPacientePorIdUsuario(int idUsuario)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("SELECT Id FROM Paciente WHERE IdUsuario = @idUsuario");
                    datos.SetearParametro("@idUsuario", idUsuario);
                    datos.EjecutarLectura();

                    if (datos.Lector.Read() && !Convert.IsDBNull(datos.Lector["Id"]))
                        return Convert.ToInt32(datos.Lector["Id"]);

                    return -1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener ID del paciente.", ex);
                }
            }
        }

        public bool ExisteCorreo(string emailExistente)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("Select Count(*) From Paciente Where Email = @Email");
                    datos.SetearParametro("@email", emailExistente);

                    int count = datos.EjecutarAccionEscalar();
                    return count > 0;
                }
                catch (Exception ex)
                {

                    throw new Exception("Correo existente");
                }
            }
        }
        public void BajaLogica(int idUsuario)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("UPDATE Usuario SET Activo = 0 WHERE Id = @Id");
                datos.SetearParametro("@Id", idUsuario);
                datos.EjecutarAccion();
            }
        }



        public Paciente ObtenerPorId(int id)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
                    SELECT
                        P.Id,
                        P.Nombre,
                        P.Apellido,
                        P.Dni,
                        P.Email,
                        P.Telefono,
                        P.IdUsuario,
                        U.NombreUsuario,
                        U.Clave,
                        U.IdRol,
                        R.Descripcion AS RolDescripcion,
                        U.Activo AS ActivoUsuario
                    FROM Paciente P
                    LEFT JOIN Usuario U ON P.IdUsuario = U.Id
                    LEFT JOIN Rol R ON U.IdRol = R.Id
                    WHERE P.Id = @Id");

                datos.SetearParametro("@Id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    Paciente p = new Paciente
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Dni = datos.Lector["Dni"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Telefono = datos.Lector["Telefono"].ToString(),
                        
                    };
                    if (!Convert.IsDBNull(datos.Lector["IdUsuario"]))
                    {
                        p.Usuario = new Usuario
                        {
                            Id = (int)datos.Lector["IdUsuario"],
                            NombreUsuario = datos.Lector["NombreUsuario"].ToString(),
                            Clave = datos.Lector["Clave"].ToString(),
                            Activo = (bool)datos.Lector["ActivoUsuario"],
                            Rol = new Rol
                            {
                                Id = (int)datos.Lector["IdRol"],
                                Descripcion = datos.Lector["RolDescripcion"].ToString()
                            }
                        };
                    }

                    return p;
                }

                return null;
            }
        }
        public void CambiarEstado(int idUsuario, bool activo)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("UPDATE Usuario SET Activo = @Activo WHERE Id = @Id");
                datos.SetearParametro("@Activo", activo);
                datos.SetearParametro("@Id", idUsuario);
                datos.EjecutarAccion();
            }
        }
        public List<Paciente> ListarFiltrado(string filtro)
        {
            List<Paciente> lista = new List<Paciente>();
            Datos datos = new Datos();

            try
            {
                string consulta = @"
            SELECT 
                p.Id,
                p.Nombre,
                p.Apellido,
                p.Dni,
                p.Email,
                p.Telefono,
                p.IdCobertura,
                p.IdUsuario,
                u.NombreUsuario,
                u.Clave,
                u.Activo,
                u.IdRol
            FROM Paciente p
            INNER JOIN Usuario u ON u.Id = p.IdUsuario
        ";

                if (filtro == "activos")
                    consulta += " WHERE u.Activo = 1";
                else if (filtro == "inactivos")
                    consulta += " WHERE u.Activo = 0";

                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Paciente aux = new Paciente
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Dni = datos.Lector["Dni"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Telefono = datos.Lector["Telefono"].ToString(),
                        

                        Cobertura = new Cobertura
                        {
                            Id = (int)datos.Lector["IdCobertura"]
                        },

                        Usuario = new Usuario
                        {
                            Id = (int)datos.Lector["IdUsuario"],
                            NombreUsuario = datos.Lector["NombreUsuario"].ToString(),
                            Clave = datos.Lector["Clave"].ToString(),
                            Activo = (bool)datos.Lector["Activo"],
                            Rol = new Rol
                            {
                                Id = (int)datos.Lector["IdRol"]
                            }
                        }
                    };

                    lista.Add(aux);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }



    }

}

