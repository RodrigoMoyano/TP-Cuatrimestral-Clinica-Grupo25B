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
                SELECT M.Id, M.Nombre, M.Apellido, M.Matricula, 
                       M.Email, M.Telefono,
                       E.Id AS IdEspecialidad, E.Descripcion AS Especialidad
                FROM Medico M
                LEFT JOIN MedicoEspecialidad ME ON M.Id = ME.IdMedico
                LEFT JOIN Especialidad E ON ME.IdEspecialidad = E.Id
            ");
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Medico aux = new Medico
                        {
                            IdMedico = (int)datos.Lector["Id"],
                            Nombre = datos.Lector["Nombre"].ToString(),
                            Apellido = datos.Lector["Apellido"].ToString(),
                            Matricula = datos.Lector["Matricula"].ToString(),
                            Email = datos.Lector["Email"].ToString(),
                            Telefono = datos.Lector["Telefono"].ToString(),
                            IdEspecialidad = datos.Lector["IdEspecialidad"] != DBNull.Value ? (int)datos.Lector["IdEspecialidad"] : 0,
                            Especialidad = new List<Especialidad>
                    {
                        new Especialidad
                        {
                            IdEspecialidad = datos.Lector["IdEspecialidad"] != DBNull.Value ? (int)datos.Lector["IdEspecialidad"] : 0,
                            Descripcion = datos.Lector["Especialidad"] != DBNull.Value ? datos.Lector["Especialidad"].ToString() : "Sin especialidad"
                        }
                    }
                        };
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

        public void Agregar(Medico nuevo)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    // 1️⃣ Insertamos el médico y obtenemos el ID generado
                    datos.SetearConsulta(@"
                INSERT INTO Medico (Nombre, Apellido, Matricula, Email, Telefono)
                VALUES (@Nombre, @Apellido, @Matricula, @Email, @Telefono);
                SELECT SCOPE_IDENTITY();
            ");
                    datos.SetearParametro("@Nombre", nuevo.Nombre);
                    datos.SetearParametro("@Apellido", nuevo.Apellido);
                    datos.SetearParametro("@Matricula", nuevo.Matricula);
                    datos.SetearParametro("@Email", nuevo.Email);
                    datos.SetearParametro("@Telefono", nuevo.Telefono);

                    int nuevoId = datos.EjecutarAccionEscalar();

                    // 2️⃣ Insertamos la relación con la especialidad (si hay una seleccionada)
                    if (nuevo.IdEspecialidad > 0)
                    {
                        datos.SetearConsulta("INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (@IdMedico, @IdEspecialidad)");
                        datos.SetearParametro("@IdMedico", nuevoId);
                        datos.SetearParametro("@IdEspecialidad", nuevo.IdEspecialidad);
                        datos.EjecutarAccion();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar médico: " + ex.Message);
                }
            }
        }

        public void Modificar(Medico modificado)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                        UPDATE Medico 
                        SET Nombre = @Nombre, 
                            Apellido = @Apellido, 
                            Matricula = @Matricula, 
                            Email = @Email, 
                            Telefono = @Telefono
                        WHERE Id = @Id
                    ");

                    datos.SetearParametro("@Id", modificado.IdMedico);
                    datos.SetearParametro("@Nombre", modificado.Nombre);
                    datos.SetearParametro("@Apellido", modificado.Apellido);
                    datos.SetearParametro("@Matricula", modificado.Matricula);
                    datos.SetearParametro("@Email", modificado.Email ?? (object)DBNull.Value);
                    datos.SetearParametro("@Telefono", modificado.Telefono ?? (object)DBNull.Value);

                    datos.EjecutarAccion();

                    // Si se modificó la especialidad:
                    if (modificado.IdEspecialidad > 0)
                    {
                        datos.SetearConsulta("UPDATE MedicoEspecialidad SET IdEspecialidad = @IdEspecialidad WHERE IdMedico = @IdMedico");
                        datos.SetearParametro("@IdMedico", modificado.IdMedico);
                        datos.SetearParametro("@IdEspecialidad", modificado.IdEspecialidad);
                        datos.EjecutarAccion();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar médico: " + ex.Message);
                }
            }
        }


        public void Eliminar(int id)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    // Primero borro relaciones en MedicoEspecialidad
                    datos.SetearConsulta("DELETE FROM MedicoEspecialidad WHERE IdMedico = @IdMedico");
                    datos.SetearParametro("@IdMedico", id);
                    datos.EjecutarAccion();

                    // Luego borro el médico
                    datos.SetearConsulta("DELETE FROM Medico WHERE Id = @Id");
                    datos.SetearParametro("@Id", id);
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