using System;
using System.Collections.Generic;
using Dominio;

namespace AccesoDatos
{
    public class TurnoTrabajoDatos
    {
        public List<TurnoTrabajo> Listar()
        {
            List<TurnoTrabajo> lista = new List<TurnoTrabajo>();
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                        SELECT tt.Id, tt.DiaSemana, tt.HoraInicio, tt.HoraFin,
                               m.Id AS MedicoId, m.Nombre, m.Apellido
                        FROM TurnosTrabajo tt
                        INNER JOIN Medicos m ON tt.IdMedico = m.Id
                    ");
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        TurnoTrabajo t = new TurnoTrabajo
                        {
                            Id = (int)datos.Lector["Id"],
                            DiaSemana = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), datos.Lector["DiaSemana"].ToString()),
                            HoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                            HoraFin = (TimeSpan)datos.Lector["HoraFin"],
                            Medico = new Medico
                            {
                                IdMedico = (int)datos.Lector["MedicoId"],
                                Nombre = datos.Lector["Nombre"].ToString(),
                                Apellido = datos.Lector["Apellido"].ToString()
                            }
                        };
                        lista.Add(t);
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar turnos de trabajo: " + ex.Message);
                }
            }
        }

        public void Agregar(TurnoTrabajo turno)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("INSERT INTO TurnosTrabajo (IdMedico, DiaSemana, HoraInicio, HoraFin) VALUES (@IdMedico, @DiaSemana, @HoraInicio, @HoraFin)");
                    datos.SetearParametro("@IdMedico", turno.Medico.IdMedico);
                    datos.SetearParametro("@DiaSemana", turno.DiaSemana.ToString());
                    datos.SetearParametro("@HoraInicio", turno.HoraInicio);
                    datos.SetearParametro("@HoraFin", turno.HoraFin);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar turno de trabajo: " + ex.Message);
                }
            }
        }

        public void Modificar(TurnoTrabajo turno)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("UPDATE TurnosTrabajo SET IdMedico=@IdMedico, DiaSemana=@DiaSemana, HoraInicio=@HoraInicio, HoraFin=@HoraFin WHERE Id=@Id");
                    datos.SetearParametro("@IdMedico", turno.Medico.IdMedico);
                    datos.SetearParametro("@DiaSemana", turno.DiaSemana.ToString());
                    datos.SetearParametro("@HoraInicio", turno.HoraInicio);
                    datos.SetearParametro("@HoraFin", turno.HoraFin);
                    datos.SetearParametro("@Id", turno.Id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar turno de trabajo: " + ex.Message);
                }
            }
        }

        public void Eliminar(int id)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("DELETE FROM TurnosTrabajo WHERE Id=@Id");
                    datos.SetearParametro("@Id", id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar turno de trabajo: " + ex.Message);
                }
            }
        }
    }
}