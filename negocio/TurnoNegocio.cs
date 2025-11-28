using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TurnoNegocio
    {
        public List<Turno> ObtenerTodos()
        {
            List<Turno> lista = new List<Turno>();
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                        SELECT 
                            t.Id, 
                            t.Fecha, 
                            t.Hora, 
                            p.IdPaciente AS IdPaciente, 
                            p.Nombre AS NombrePaciente, 
                            m.IdMedico AS IdMedico, 
                            m.Nombre AS NombreMedico
                        FROM Turno t
                        INNER JOIN Paciente p ON p.IdPaciente = t.IdPaciente
                        INNER JOIN Medico m ON m.Id= t.Id");

                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Turno aux = new Turno
                        {
                            Id = (int)datos.Lector["Id"],
                            Fecha = (DateTime)datos.Lector["Fecha"],
                            Hora = TimeSpan.Parse(datos.Lector["Hora"].ToString()),
                            Paciente = new Paciente
                            {
                                Id = (int)datos.Lector["IdPaciente"],
                                Nombre = (string)datos.Lector["NombrePaciente"]
                            },
                            Medico = new Medico
                            {
                                Id = (int)datos.Lector["Id"],
                                Nombre = (string)datos.Lector["NombreMedico"]
                            }
                        };

                        lista.Add(aux);
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener la lista de turnos", ex);
                }
            }
        }
        public List<TimeSpan> ObtenerHorariosOcupados(int idMedico, DateTime fecha)
        {
            List<TimeSpan> lista = new List<TimeSpan>();
            Datos datos = new Datos();

            try
            {
                datos.SetearConsulta("SELECT Hora FROM Turno WHERE IdMedico = @idMedico AND Fecha = @fecha");
                datos.SetearParametro("@idMedico", idMedico);
                datos.SetearParametro("@fecha", fecha);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add((TimeSpan)datos.Lector["Hora"]);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        //agregar un turno
        public void Agregar(Turno turno)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("INSERT INTO Turno (Fecha, Hora, IdPaciente, IdMedico) VALUES (@Fecha, @Hora, @IdPaciente, @IdMedico)");
                    datos.SetearParametro("@Fecha", turno.Fecha);
                    datos.SetearParametro("@Hora", turno.Hora);
                    datos.SetearParametro("@IdPaciente", turno.Paciente.Id);
                    datos.SetearParametro("@IdMedico", turno.Medico.Id);

                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar el turno", ex);
                }
            }
        }


        public void Agregar(int idPaciente, int idMedico, int idEspecialidad, DateTime fecha, TimeSpan hora, string observaciones)
        {
            using (Datos datos = new Datos())
            {
                if (!TurnoDisponible(idMedico, fecha, hora))
                    throw new Exception("El horario seleccionado ya está ocupado.");
                try
                {
                    datos.SetearConsulta(@"
                INSERT INTO Turno (Fecha, Hora, IdPaciente, IdMedico, IdEspecialidad, IdEstadoTurno, Observaciones)
                VALUES (@Fecha, @Hora, @IdPaciente, @IdMedico, @IdEspecialidad, @IdEstadoTurno, @Observaciones)");

                    datos.SetearParametro("@Fecha", fecha);
                    datos.SetearParametro("@Hora", hora);
                    datos.SetearParametro("@IdPaciente", idPaciente);
                    datos.SetearParametro("@IdMedico", idMedico);
                    datos.SetearParametro("@IdEspecialidad", idEspecialidad);
                    datos.SetearParametro("@IdEstadoTurno", 1); // 1 = Pendiente
                    datos.SetearParametro("@Observaciones", observaciones);

                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar el turno", ex);
                }
            }
        }

        //obetner turnos por paciente
        public List<Turno> ObtenerPorPaciente(int idPaciente)
        {
            List<Turno> lista = new List<Turno>();
            using (Datos datos = new Datos())
            {
                try
                {
                    
                        datos.SetearConsulta(@"
                            SELECT
                                t.Id, t.Fecha, t.Hora,
                                p.Id AS IdPaciente, p.Nombre AS NombrePaciente,
                                m.Id AS IdMedico, m.Nombre AS NombreMedico
                            FROM Turno t
                            INNER JOIN Paciente p ON p.Id = t.IdPaciente
                            INNER JOIN Medico m ON m.Id = t.IdMedico
                            WHERE p.Id = @IdPaciente");

                    datos.SetearParametro("@IdPaciente", idPaciente);
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Turno aux = new Turno
                        {
                            Id = (int)datos.Lector["Id"],
                            Fecha = (DateTime)datos.Lector["Fecha"],
                            Hora = TimeSpan.Parse(datos.Lector["Hora"].ToString()),
                            Paciente = new Paciente
                            {
                                Id = (int)datos.Lector["IdPaciente"],
                                Nombre = (string)datos.Lector["NombrePaciente"]
                            },
                            Medico = new Medico
                            {
                                Id = (int)datos.Lector["Id"],
                                Nombre = (string)datos.Lector["NombreMedico"]
                            }
                        };

                        lista.Add(aux);
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los turnos del paciente", ex);
                }
            }
        }

        //obtener turnos por médico
        public List<Turno> ObtenerPorMedico(int idMedico)
        {
            List<Turno> lista = new List<Turno>();
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                        SELECT 
                            t.Id, t.Fecha, t.Hora,
                            p.Id AS IdPaciente, p.Nombre AS NombrePaciente,
                            m.Id AS IdMedico, m.Nombre AS NombreMedico
                        FROM Turno t
                        INNER JOIN Paciente p ON p.Id = t.IdPaciente
                        INNER JOIN Medico m ON m.Id = t.IdMedico
                        WHERE m.Id = @IdMedico");

                    datos.SetearParametro("@Id", idMedico);
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Turno aux = new Turno
                        {
                            Id = (int)datos.Lector["Id"],
                            Fecha = (DateTime)datos.Lector["Fecha"],
                            Hora = TimeSpan.Parse(datos.Lector["Hora"].ToString()),
                            Paciente = new Paciente
                            {
                                Id = (int)datos.Lector["IdPaciente"],
                                Nombre = (string)datos.Lector["NombrePaciente"]
                            },
                            Medico = new Medico
                            {
                                Id = (int)datos.Lector["Id"],
                                Nombre = (string)datos.Lector["NombreMedico"]
                            }
                        };

                        lista.Add(aux);
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los turnos del médico", ex);
                }
            }
        }

        //sp Ver turnos
        public List<VerTurno> spVerTurno()
        {
            List<VerTurno> lista = new List<VerTurno>();

            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearSp("SpVerTurno");

                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        VerTurno aux = new VerTurno();

                        aux.Fecha = (DateTime)datos.Lector["Fecha"];
                        aux.Hora = (TimeSpan)datos.Lector["Hora"];
                        aux.Paciente = (string)datos.Lector["Paciente"];
                        aux.Medico = (string)datos.Lector["Medico"];
                        aux.Especialidad = (string)datos.Lector["Especialidad"];
                        aux.Estado = (string)datos.Lector["Estado"];

                        lista.Add(aux);
                    }

                    return lista;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            
        }

        //Actualizar turno a cancelado
        public void CancelarTurno(int idTurno)
        {
            const int ESTADO_CANCELADO = 3;

            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("Update Turno Set IdEstadoTurno = @IdEstado Where Id = @IdTurno");
                    datos.SetearParametro("@IdEstado", ESTADO_CANCELADO);
                    datos.SetearParametro("@IdTurno", idTurno);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {

                    throw new Exception("Error al cancelar turno.", ex);
                }
            }
        }

        //Para ver los turnos de cada paciente
        // public List<Turno> ListarPorPaciente(int idPaciente)
        // {
        //     List<Turno> lista = new List<Turno>();

        //     using (Datos datos = new Datos())
        //     {
        //         try
        //         {
        //             datos.SetearConsulta("Update Turno Set IdEstadoTurno = @IdEstado Where Id = @IdTurno");
        //             datos.SetearParametro("@IdEstado", ESTADO_CANCELADO);
        //             datos.SetearParametro("@IdTurno", idTurno);
        //             datos.EjecutarAccion();
        //         }
        //         catch (Exception ex)
        //         {

        //             throw new Exception("Error al listar paciente.", ex);
        //         }
        //     }
        // }
        public List<Turno> ListarPorPaciente(int idPaciente)
        {
            List<Turno> lista = new List<Turno>();

            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                SELECT 
                    t.Id,
                    t.Fecha,
                    t.Hora,
                    e.Descripcion AS Estado,
                    esp.Descripcion AS Especialidad,
                    m.Nombre + ' ' + m.Apellido AS Medico
                    FROM Turno t
                    INNER JOIN EstadoTurno e ON t.IdEstadoTurno = e.Id
                    INNER JOIN Especialidad esp ON t.IdEspecialidad = esp.Id
                    INNER JOIN Medico m ON t.IdMedico = m.Id
                    WHERE t.IdPaciente = @IdPaciente
                    ORDER BY t.Fecha ASC, t.Hora ASC");

                    datos.SetearParametro("@IdPaciente", idPaciente);
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Turno t = new Turno
                        {
                            Id = (int)datos.Lector["Id"],
                            Fecha = (DateTime)datos.Lector["Fecha"],
                            Hora = (TimeSpan)datos.Lector["Hora"],

                            Estado = new EstadoTurno
                            {
                                Descripcion = datos.Lector["Estado"].ToString()
                            },

                            Especialidad = new Especialidad
                            {
                                Descripcion = datos.Lector["Especialidad"].ToString()
                            },

                            Medico = new Medico
                            {
                                Nombre = datos.Lector["Medico"].ToString()
                            }
                        };

                        lista.Add(t);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar turnos del paciente: " + ex.Message);
                }
            }
            return lista;
        }
        public bool TurnoDisponible(int idMedico, DateTime fecha, TimeSpan hora)
        {
            Datos datos = new Datos();
            try
            {
                datos.SetearConsulta("SELECT COUNT(*) FROM Turno WHERE IdMedico = @idMedico AND Fecha = @fecha AND Hora = @hora");
                datos.SetearParametro("@idMedico", idMedico);
                datos.SetearParametro("@fecha", fecha.Date);
                datos.SetearParametro("@hora", hora);

                datos.EjecutarLectura();
                if (datos.Lector.Read())
                {
                    int count = (int)datos.Lector[0];
                    return count == 0;
                }

                return false;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public Turno BuscarPorId(int idTurno)
        {
            Datos datos = new Datos();

            try
            {
                datos.SetearConsulta(@"SELECT t.Id, t.Fecha, t.Hora, t.Observaciones, t.IdPaciente, t.IdMedico, t.IdEspecialidad, t.IdEstadoTurno
                FROM Turno t
                WHERE t.Id = @id
                ");

                datos.SetearParametro("@id", idTurno);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    Turno turno = new Turno();
                    turno.Id = (int)datos.Lector["Id"];
                    turno.Fecha = (DateTime)datos.Lector["Fecha"];
                    turno.Hora = (TimeSpan)datos.Lector["Hora"];
                    turno.Observaciones = datos.Lector["Observaciones"] != DBNull.Value
                                          ? datos.Lector["Observaciones"].ToString()
                                          : "";

                    
                    turno.Paciente = new Paciente { Id = (int)datos.Lector["IdPaciente"] };
                    turno.Medico = new Medico { Id = (int)datos.Lector["IdMedico"] };
                    turno.Especialidad = new Especialidad { Id = (int)datos.Lector["IdEspecialidad"] };
                    turno.Estado = new EstadoTurno { Id = (int)datos.Lector["IdEstadoTurno"] };

                    return turno;
                }

                return null;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void Cancelar(int idTurno)
        {
            Datos datos = new Datos();
            try
            {
                datos.SetearConsulta("UPDATE Turno SET IdEstadoTurno = 3 WHERE Id = @id"); 
                datos.SetearParametro("@id", idTurno);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        //Para filtrar turnos
        
        public List<Turno> ListarPorPacienteFiltrado(int idPaciente, string estado)
        {
            Datos datos = new Datos();
            List<Turno> lista = new List<Turno>();

            try
            {
                string consulta = @"
                    SELECT 
                        t.Id,
                        t.Fecha,
                        t.Hora,
                        t.Observaciones,
                        e.Descripcion AS Estado,
                        esp.Descripcion AS Especialidad,
                        m.Nombre + ' ' + m.Apellido AS Medico
                    FROM Turno t
                    INNER JOIN EstadoTurno e ON t.IdEstadoTurno = e.Id
                    INNER JOIN Especialidad esp ON t.IdEspecialidad = esp.Id
                    INNER JOIN Medico m ON t.IdMedico = m.Id
                    WHERE t.IdPaciente = @idPaciente";

                if (!string.IsNullOrEmpty(estado))
                {
                    consulta += " AND e.Descripcion = @estado";
                }

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@idPaciente", idPaciente);

                if (!string.IsNullOrEmpty(estado))
                    datos.SetearParametro("@estado", estado);

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Turno aux = new Turno
                    {
                        Id = (int)datos.Lector["Id"],
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        Hora = TimeSpan.Parse(datos.Lector["Hora"].ToString()),
                        Observaciones = datos.Lector["Observaciones"].ToString(),
                        Estado = new EstadoTurno { Descripcion = datos.Lector["Estado"].ToString() },
                        Especialidad = new Especialidad { Descripcion = datos.Lector["Especialidad"].ToString() },
                        Medico = new Medico { Nombre = datos.Lector["Medico"].ToString() }
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
