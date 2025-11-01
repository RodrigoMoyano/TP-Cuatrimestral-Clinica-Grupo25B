﻿using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class TurnoDatos
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
                            p.Id AS IdPaciente, 
                            p.Nombre AS NombrePaciente, 
                            m.Id AS IdMedico, 
                            m.Nombre AS NombreMedico
                        FROM Turnos t
                        INNER JOIN Pacientes p ON p.Id = t.IdPaciente
                        INNER JOIN Medicos m ON m.Id = t.IdMedico");

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
                                Id = (int)datos.Lector["IdMedico"],
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

        // ✅ Método para agregar un turno
        public void Agregar(Turno turno)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("INSERT INTO Turnos (Fecha, Hora, IdPaciente, IdMedico) VALUES (@Fecha, @Hora, @IdPaciente, @IdMedico)");
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

        // ✅ Método para obtener turnos por paciente
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
                        FROM Turnos t
                        INNER JOIN Pacientes p ON p.Id = t.IdPaciente
                        INNER JOIN Medicos m ON m.Id = t.IdMedico
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
                                Id = (int)datos.Lector["IdMedico"],
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

        // ✅ Método para obtener turnos por médico
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
                        FROM Turnos t
                        INNER JOIN Pacientes p ON p.Id = t.IdPaciente
                        INNER JOIN Medicos m ON m.Id = t.IdMedico
                        WHERE m.Id = @IdMedico");

                    datos.SetearParametro("@IdMedico", idMedico);
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
                                Id = (int)datos.Lector["IdMedico"],
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
    }
}