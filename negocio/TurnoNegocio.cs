using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TurnoNegocio
    {
        //CORREGIDO joins y columnas de Paciente/Medico
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
                    p.Id AS IdPaciente,
                    p.Nombre AS NombrePaciente,
                    p.Apellido AS ApellidoPaciente,
                    m.Id AS IdMedico,
                    m.Nombre AS NombreMedico,
                    m.Apellido AS ApellidoMedico,
                    esp.Id AS IdEspecialidad,
                    esp.Descripcion AS Especialidad,
                    e.Id AS IdEstado,
                    e.Descripcion AS Estado
                FROM Turno t
                INNER JOIN Paciente p ON p.Id = t.IdPaciente
                INNER JOIN Medico m ON m.Id = t.IdMedico
                INNER JOIN Especialidad esp ON esp.Id = t.IdEspecialidad
                INNER JOIN EstadoTurno e ON e.Id = t.IdEstadoTurno
                ORDER BY t.Fecha, t.Hora");

                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Turno aux = new Turno
                        {
                            Id = (int)datos.Lector["Id"],
                            Fecha = (DateTime)datos.Lector["Fecha"],
                            Hora = datos.Lector["Hora"] != DBNull.Value &&
                                   TimeSpan.TryParse(datos.Lector["Hora"].ToString(), out var ts)
                                   ? ts : TimeSpan.Zero,

                            Paciente = new Paciente
                            {
                                Id = (int)datos.Lector["IdPaciente"],
                                Nombre = datos.Lector["NombrePaciente"].ToString(),
                                Apellido = datos.Lector["ApellidoPaciente"].ToString()
                            },

                            Medico = new Medico
                            {
                                Id = (int)datos.Lector["IdMedico"],
                                Nombre = datos.Lector["NombreMedico"].ToString(),
                                Apellido = datos.Lector["ApellidoMedico"].ToString()
                            },

                            Especialidad = new Especialidad
                            {
                                Id = (int)datos.Lector["IdEspecialidad"],
                                Descripcion = datos.Lector["Especialidad"].ToString()
                            },

                            Estado = new EstadoTurno
                            {
                                Id = (int)datos.Lector["IdEstado"],
                                Descripcion = datos.Lector["Estado"].ToString()
                            }
                        };

                        lista.Add(aux);
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener la lista de todos los turnos", ex);
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

        //Metodo para Modificar o Reprogramar turno
        public void Modificar(int idTurno, int idMedico, int IdEspecialidad, DateTime fecha, TimeSpan hora, string observaciones)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"Update Turno Set IdMedico = @IdMedico, IdEspecialidad = @IdEspecialidad, Fecha = @Fecha, Hora = @Hora, Observaciones = @Observaciones, IdEstadoTurno = 2 Where Id = @Id");

                    datos.SetearParametro("@Id", idTurno);
                    datos.SetearParametro("@IdMedico", idMedico);
                    datos.SetearParametro("@IdEspecialidad", IdEspecialidad);
                    datos.SetearParametro("@Fecha", fecha);
                    datos.SetearParametro("@Hora", hora);
                    datos.SetearParametro("@Observaciones", observaciones);

                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {

                    throw new Exception("Error al modificar el turno", ex);
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
                    t.Id,
                    t.Fecha,
                    t.Hora,
                    p.Id AS PacienteId,
                    p.Nombre AS PacienteNombre,
                    p.Apellido AS PacienteApellido,
                    m.Id AS MedicoId,
                    m.Nombre AS MedicoNombre,
                    m.Apellido AS MedicoApellido
                FROM Turno t
                INNER JOIN Paciente p ON p.Id = t.IdPaciente
                INNER JOIN Medico m ON m.Id = t.IdMedico
                WHERE t.IdPaciente = @IdPaciente
                ORDER BY t.Fecha ASC, t.Hora ASC");

                    datos.SetearParametro("@IdPaciente", idPaciente);
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Turno turno = new Turno
                        {
                            Id = (int)datos.Lector["Id"],
                            Fecha = (DateTime)datos.Lector["Fecha"],
                            Hora = datos.Lector["Hora"] != DBNull.Value &&
                                   TimeSpan.TryParse(datos.Lector["Hora"].ToString(), out var ts)
                                   ? ts
                                   : TimeSpan.Zero,

                            Paciente = new Paciente
                            {
                                Id = (int)datos.Lector["PacienteId"],
                                Nombre = datos.Lector["PacienteNombre"].ToString(),
                                Apellido = datos.Lector["PacienteApellido"].ToString()
                            },

                            Medico = new Medico
                            {
                                Id = (int)datos.Lector["MedicoId"],
                                Nombre = datos.Lector["MedicoNombre"].ToString(),
                                Apellido = datos.Lector["MedicoApellido"].ToString()
                            }
                        };

                        lista.Add(turno);
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
                    T.Id,
                    T.Fecha,
                    T.Hora,
                    T.Observaciones,

                    P.Id AS PacienteId,
                    P.Nombre AS PacienteNombre,
                    P.Apellido AS PacienteApellido,

                    M.Id AS MedicoId,
                    M.Nombre AS MedicoNombre,
                    M.Apellido AS MedicoApellido,

                    ET.Id AS EstadoId,
                    ET.Descripcion AS EstadoDescripcion

                FROM Turno T
                INNER JOIN Paciente P       ON P.Id = T.IdPaciente
                INNER JOIN Medico M         ON M.Id = T.IdMedico
                INNER JOIN EstadoTurno ET   ON ET.Id = T.IdEstadoTurno
                WHERE T.IdMedico = @IdMedico
                ORDER BY T.Fecha ASC, T.Hora ASC
            ");

                    datos.SetearParametro("@IdMedico", idMedico);
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Turno turno = new Turno
                        {
                            Id = (int)datos.Lector["Id"],
                            Fecha = (DateTime)datos.Lector["Fecha"],
                            Hora = datos.Lector["Hora"] != DBNull.Value &&
                                   TimeSpan.TryParse(datos.Lector["Hora"].ToString(), out var ts)
                                   ? ts
                                   : TimeSpan.Zero,

                            Observaciones = datos.Lector["Observaciones"] != DBNull.Value
                                            ? datos.Lector["Observaciones"].ToString()
                                            : "",

                            Paciente = new Paciente
                            {
                                Id = (int)datos.Lector["PacienteId"],
                                Nombre = datos.Lector["PacienteNombre"].ToString(),
                                Apellido = datos.Lector["PacienteApellido"].ToString()
                            },

                            Medico = new Medico
                            {
                                Id = (int)datos.Lector["MedicoId"],
                                Nombre = datos.Lector["MedicoNombre"].ToString(),
                                Apellido = datos.Lector["MedicoApellido"].ToString()
                            },

                            Estado = new EstadoTurno
                            {
                                Id = (int)datos.Lector["EstadoId"],
                                Descripcion = datos.Lector["EstadoDescripcion"].ToString()
                            }
                        };

                        lista.Add(turno);
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

                        aux.IdTurno = (int)datos.Lector["IdTurno"];
                        aux.Fecha = (DateTime)datos.Lector["Fecha"];
                        aux.Hora = (TimeSpan)datos.Lector["Hora"];
                        aux.Paciente = (string)datos.Lector["Paciente"];
                        aux.Medico = (string)datos.Lector["Medico"];
                        aux.Especialidad = (string)datos.Lector["Especialidad"];
                        aux.TipoCobertura = datos.Lector["Cobertura"] is DBNull ? "" : (string)datos.Lector["Cobertura"];
                        aux.NombreObraSocial = datos.Lector["NombreObraSocial"] is DBNull ? "" : (string)datos.Lector["NombreObraSocial"];
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
                    m.Apellido + ' ' + m.Nombre AS MedicoCompleto
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
                            Hora = datos.Lector["Hora"] != DBNull.Value &&
                                TimeSpan.TryParse(datos.Lector["Hora"].ToString(), out var ts)
                                ? ts : TimeSpan.Zero,

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
                                Nombre = datos.Lector["MedicoCompleto"].ToString()
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
                datos.SetearConsulta(@"SELECT t.Id As IdTurno, t.Fecha, t.Hora, t.Observaciones, p.Id AS IdPaciente,p.IdCobertura,c.Tipo AS TipoCobertura,c.NombreObraSocial,t.IdMedico,t.IdEspecialidad,t.IdEstadoTurno
                                        FROM Turno t
                                        INNER JOIN Paciente p ON p.Id = t.IdPaciente
                                        INNER JOIN Cobertura c ON c.Id = p.IdCobertura
                                        WHERE t.Id = @idTurno");


                datos.SetearParametro("@IdTurno", idTurno);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    Turno turno = new Turno();

                    turno.Id = (int)datos.Lector["IdTurno"];
                    turno.Fecha = (DateTime)datos.Lector["Fecha"];
                    turno.Hora = (TimeSpan)datos.Lector["Hora"];
                    turno.Observaciones = datos.Lector["Observaciones"] != DBNull.Value
                                          ? datos.Lector["Observaciones"].ToString()
                                          : "";


                    turno.Paciente = new Paciente { Id = (int)datos.Lector["IdPaciente"] };
                    turno.Paciente.Cobertura = new Cobertura()
                    {
                        Id = (int)datos.Lector["IdCobertura"],
                        Tipo = datos.Lector["TipoCobertura"].ToString(),
                        NombreObraSocial = datos.Lector["NombreObrasocial"].ToString()
                    };
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
                (m.Nombre + ' ' + m.Apellido) AS Medico
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
                        Hora = datos.Lector["Hora"] != DBNull.Value &&
                               TimeSpan.TryParse(datos.Lector["Hora"].ToString(), out var ts)
                               ? ts
                               : TimeSpan.Zero,

                        Observaciones = datos.Lector["Observaciones"] != DBNull.Value
                                        ? datos.Lector["Observaciones"].ToString()
                                        : "",

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

                    lista.Add(aux);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public Turno ObtenerTurnoParaEditar(int idTurno)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
            SELECT 
                T.Id,
                T.Fecha,
                T.Hora,
                T.Observaciones,
                T.IdPaciente,
                T.IdMedico,
                T.IdEspecialidad,
                T.IdEstadoTurno,
                C.Id AS IdCobertura,
                C.Tipo AS TipoCobertura,
                C.NombreObraSocial
            FROM Turno T
            INNER JOIN Paciente P ON P.Id = T.IdPaciente
            INNER JOIN Cobertura C ON C.Id = P.IdCobertura
            WHERE T.Id = @id");

                    datos.SetearParametro("@id", idTurno);
                    datos.EjecutarLectura();

                    if (datos.Lector.Read())
                    {
                        Turno turno = new Turno();

                        turno.Id = (int)datos.Lector["Id"];
                        turno.Fecha = (DateTime)datos.Lector["Fecha"];
                        turno.Hora = (TimeSpan)datos.Lector["Hora"];
                        turno.Observaciones = datos.Lector["Observaciones"] is DBNull
                                              ? ""
                                              : (string)datos.Lector["Observaciones"];

                        // ----- PACIENTE + COBERTURA -----
                        turno.Paciente = new Paciente();
                        turno.Paciente.Id = (int)datos.Lector["IdPaciente"];

                        turno.Paciente.Cobertura = new Cobertura();
                        turno.Paciente.Cobertura.Id = (int)datos.Lector["IdCobertura"];
                        turno.Paciente.Cobertura.Tipo = datos.Lector["TipoCobertura"].ToString();
                        turno.Paciente.Cobertura.NombreObraSocial = datos.Lector["NombreObraSocial"].ToString();

                        // ----- MÉDICO / ESP / ESTADO -----
                        turno.Medico = new Medico { Id = (int)datos.Lector["IdMedico"] };
                        turno.Especialidad = new Especialidad { Id = (int)datos.Lector["IdEspecialidad"] };
                        turno.Estado = new EstadoTurno { Id = (int)datos.Lector["IdEstadoTurno"] };

                        return turno;
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el turno para editar", ex);
                }
            }
        }

        public List<VerTurno> ListarVerTurnos()
        {
            List<VerTurno> lista = new List<VerTurno>();

            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                SELECT 
                    t.Id,
                    t.Fecha,
                    t.Hora,
                    p.Nombre + ' ' + p.Apellido AS Paciente,
                    m.Nombre + ' ' + m.Apellido AS Medico,
                    esp.Descripcion AS Especialidad,
                    et.Descripcion AS Estado
                FROM Turno t
                INNER JOIN Paciente     p   ON p.Id = t.IdPaciente
                INNER JOIN Medico       m   ON m.Id = t.IdMedico
                INNER JOIN Especialidad esp ON esp.Id = t.IdEspecialidad
                INNER JOIN EstadoTurno  et  ON et.Id = t.IdEstadoTurno
                ORDER BY t.Fecha ASC, t.Hora ASC");

                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        VerTurno aux = new VerTurno
                        {
                            IdTurno = (int)datos.Lector["Id"],
                            Fecha = (DateTime)datos.Lector["Fecha"],
                            Hora = datos.Lector["Hora"] != DBNull.Value &&
                                   TimeSpan.TryParse(datos.Lector["Hora"].ToString(), out var ts)
                                   ? ts
                                   : TimeSpan.Zero,
                            Paciente = datos.Lector["Paciente"].ToString(),
                            Medico = datos.Lector["Medico"].ToString(),
                            Especialidad = datos.Lector["Especialidad"].ToString(),
                            Estado = datos.Lector["Estado"].ToString()
                        };

                        lista.Add(aux);
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar turnos para administración.", ex);
                }
            }
        }

        public void CambiarEstado(int idTurno, int idEstado)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("UPDATE Turno SET IdEstadoTurno = @IdEstado WHERE Id = @IdTurno");
                    datos.SetearParametro("@IdEstado", idEstado);
                    datos.SetearParametro("@IdTurno", idTurno);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cambiar estado del turno.", ex);
                }
            }
        }

        public void ActualizarObservaciones(int idTurno, string observaciones)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("UPDATE Turno SET Observaciones = @obs WHERE Id = @id");
                    datos.SetearParametro("@obs", observaciones);
                    datos.SetearParametro("@id", idTurno);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar las observaciones: " + ex.Message);
                }
            }
        }

    }
}