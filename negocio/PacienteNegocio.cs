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
                    datos.SetearConsulta(@" Insert Into Usuario (NombreUsuario, Clave, IdRol, Activo) Values(@NombreUsuario, @Clave, 3, 1) Select Scope_Identity()");

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
                datos.SetearConsulta("SELECT P.Id, P.Nombre, P.Apellido, P.Dni, P.Email, P.Telefono, P.IdUsuario, U.NombreUsuario FROM Paciente P LEFT JOIN Usuario U ON P.IdUsuario = U.Id");
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
                        Telefono = datos.Lector["Telefono"].ToString()
                    };
                    if (datos.Lector["IdUsuario"] != DBNull.Value)
                    {
                        p.Usuario = new Usuario
                        {
                            Id = (int)datos.Lector["IdUsuario"],
                            NombreUsuario = datos.Lector["NombreUsuario"].ToString()
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
                /*datos.SetearConsulta("INSERT INTO Paciente (Nombre, Apellido, Dni, Email, Telefono, IdUsuario) " +
                                     "VALUES (@Nombre, @Apellido, @Dni, @Email, @Telefono, @IdUsuario");

                datos.SetearParametro("@Nombre", paciente.Nombre);
                datos.SetearParametro("@Apellido", paciente.Apellido);
                datos.SetearParametro("@Dni", paciente.Dni);
                datos.SetearParametro("@Email", paciente.Email);
                datos.SetearParametro("@Telefono", paciente.Telefono);

                datos.SetearParametro("@IdUsuario", paciente.IdUsuario.IdUsuario);

                datos.EjecutarAccion();*/
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
                datos.SetearParametro("@Id", paciente.Id);

                datos.SetearParametro("@IdUsuario", paciente.Id);

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
                finally
                {
                    datos.CerrarConexion();
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

    }

}

