using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;


namespace Negocio
{
    public class MedicoNegocio
    {
        public List<Medico> Listar()
        {
            List<Medico> lista = new List<Medico>();

            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                SELECT Id, Nombre, Apellido, Matricula, Telefono, Email, IdUsuario
                FROM Medico
                WHERE Activo = 1");

                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Medico aux = new Medico
                        {
                            Id = (int)datos.Lector["Id"],
                            Nombre = datos.Lector["Nombre"].ToString(),
                            Apellido = datos.Lector["Apellido"].ToString(),
                            Matricula = datos.Lector["Matricula"].ToString(),
                            Telefono = datos.Lector["Telefono"].ToString(),
                            Email = datos.Lector["Email"].ToString(),
                            IdUsuario = datos.Lector["IdUsuario"] != DBNull.Value
                                ? Convert.ToInt32(datos.Lector["IdUsuario"])
                                : 0
                        };

                        // Cargar especialidades
                        aux.Especialidad = ObtenerEspecialidadesDeMedico(aux.Id);

                        lista.Add(aux);
                    }
                }
                finally
                {
                    datos.CerrarConexion();
                }
            }

            return lista;
        }

        public int Agregar(Medico nuevo)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    
                    datos.SetearConsulta(@"
                INSERT INTO Medico (Nombre, Apellido, Matricula, Telefono, Email, IdUsuario)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @Apellido, @Matricula, @Telefono, @Email, @IdUsuario)");

                    datos.SetearParametro("@Nombre", nuevo.Nombre);
                    datos.SetearParametro("@Apellido", nuevo.Apellido);
                    datos.SetearParametro("@Matricula", nuevo.Matricula);
                    datos.SetearParametro("@Telefono", nuevo.Telefono);
                    datos.SetearParametro("@Email", nuevo.Email);
                    datos.SetearParametro("@IdUsuario", nuevo.IdUsuario);

                    int idMedico = datos.EjecutarAccionEscalar();

                   
                    if (nuevo.Especialidad != null)
                    {
                        foreach (var esp in nuevo.Especialidad)
                        {
                            datos.SetearConsulta("INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (@IdMedico, @IdEspecialidad)");
                            datos.SetearParametro("@IdMedico", idMedico);
                            datos.SetearParametro("@IdEspecialidad", esp.Id);
                            datos.EjecutarAccion();
                        }
                    }

                    return idMedico;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar médico: " + ex.Message);
                }
            }
        }

        /* public void Modificar(Medico modificado)
         {
             using (Datos datos = new Datos())
             {
                 try
                 {
                     datos.SetearConsulta("UPDATE Medico SET Nombre=@Nombre, Apellido=@Apellido, Matricula=@Matricula, IdEspecialidad=@IdUsuario WHERE Id=@Id");
                     datos.SetearParametro("@Id", modificado.Id);
                     datos.SetearParametro("@Nombre", modificado.Nombre);
                     datos.SetearParametro("@Apellido", modificado.Apellido);
                     datos.SetearParametro("@Matricula", modificado.Matricula);
                     datos.SetearParametro("@IdUsuario", modificado.IdUsuario);
                     datos.EjecutarAccion();
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("Error al modificar médico: " + ex.Message);
                 }
             }
         }*/

        /*public void Eliminar(int id)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("DELETE FROM Medico WHERE Id=@Id");
                    datos.SetearParametro("@Id", id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar médico: " + ex.Message);
                }
            }
        }*/
        public List<Especialidad> ObtenerEspecialidadesDeMedico(int idMedico)
        {
            using (var datos = new Datos())
            {
                datos.SetearConsulta(@"
            SELECT e.Id, e.Descripcion
            FROM MedicoEspecialidad me
            INNER JOIN Especialidad e ON e.Id = me.IdEspecialidad
            WHERE me.IdMedico = @id");

                datos.SetearParametro("@id", idMedico);
                datos.EjecutarLectura();

                List<Especialidad> lista = new List<Especialidad>();

                while (datos.Lector.Read())
                {
                    lista.Add(new Especialidad
                    {
                        Id = (int)datos.Lector["Id"],
                        Descripcion = datos.Lector["Descripcion"].ToString()
                    });
                }

                return lista;
            }
        }
        public Dominio.Medico BuscarPorId(int idMedico)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                SELECT Id, Nombre, Apellido, Matricula, Telefono, Email, IdUsuario
                FROM Medico
                WHERE Id = @Id AND Activo = 1"); // ✅ solo médicos activos

                    datos.SetearParametro("@Id", idMedico);
                    datos.EjecutarLectura();

                    Dominio.Medico medico = null; // ✅ tipo explícito para evitar conflictos

                    if (datos.Lector.Read())
                    {
                        medico = new Dominio.Medico
                        {
                            Id = (int)datos.Lector["Id"],
                            Nombre = datos.Lector["Nombre"].ToString(),
                            Apellido = datos.Lector["Apellido"].ToString(),
                            Matricula = datos.Lector["Matricula"].ToString(),
                            Telefono = datos.Lector["Telefono"].ToString(),
                            Email = datos.Lector["Email"].ToString(),
                            IdUsuario = datos.Lector["IdUsuario"] != DBNull.Value
                                ? Convert.ToInt32(datos.Lector["IdUsuario"])
                                : 0
                        };
                    }

                    datos.CerrarConexion();

                    // ✅ Cargar especialidades desde tabla intermedia
                    if (medico != null)
                        medico.Especialidad = ObtenerEspecialidadesDeMedico(medico.Id);

                    return medico;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar médico: " + ex.Message);
                }
            }
        }

        public List<Medico> ListarPorEspecialidad(int idEspecialidad)
        {
            List<Medico> lista = new List<Medico>();
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
            SELECT M.Id, M.Nombre, M.Apellido
            FROM Medico M
            INNER JOIN MedicoEspecialidad ME ON M.Id = ME.IdMedico
            WHERE ME.IdEspecialidad = @id");

                datos.SetearParametro("@id", idEspecialidad);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Medico
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString()
                    });
                }
            }
            return lista;
        }
        public void Modificar(Medico modificado)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    // 1) Actualizar datos básicos del médico
                    datos.SetearConsulta(@"
                UPDATE Medico 
                SET Nombre=@Nombre, Apellido=@Apellido, Matricula=@Matricula, Telefono=@Telefono, Email=@Email
                WHERE Id=@Id");

                    datos.SetearParametro("@Id", modificado.Id);
                    datos.SetearParametro("@Nombre", modificado.Nombre);
                    datos.SetearParametro("@Apellido", modificado.Apellido);
                    datos.SetearParametro("@Matricula", modificado.Matricula);
                    datos.SetearParametro("@Telefono", modificado.Telefono);
                    datos.SetearParametro("@Email", modificado.Email);
                    datos.EjecutarAccion();

                    // 2) Actualizar especialidades en tabla intermedia
                    // Primero eliminamos las existentes
                    datos.SetearConsulta("DELETE FROM MedicoEspecialidad WHERE IdMedico=@IdMedico");
                    datos.SetearParametro("@IdMedico", modificado.Id);
                    datos.EjecutarAccion();

                    // Luego insertamos las nuevas
                    if (modificado.Especialidad != null)
                    {
                        foreach (var esp in modificado.Especialidad)
                        {
                            datos.SetearConsulta("INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (@IdMedico, @IdEspecialidad)");
                            datos.SetearParametro("@IdMedico", modificado.Id);
                            datos.SetearParametro("@IdEspecialidad", esp.Id);
                            datos.EjecutarAccion();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar médico: " + ex.Message);
                }
            }
        }

        public void Eliminar(int idMedico)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    // ✅ Eliminación lógica: marcamos como inactivo
                    datos.SetearConsulta("UPDATE Medico SET Activo = 0 WHERE Id = @Id");
                    datos.SetearParametro("@Id", idMedico);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar médico: " + ex.Message);
                }
            }
        }

    }
}