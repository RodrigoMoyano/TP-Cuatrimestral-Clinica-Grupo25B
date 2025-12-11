using Dominio;
using System;
using System.Collections.Generic;

namespace Negocio
{
    public class TurnoTrabajoNegocio : Datos
    {

        public List<TurnoTrabajo> Listar()
        {
            List<TurnoTrabajo> lista = new List<TurnoTrabajo>();

            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                        SELECT 
                            tt.Id,
                            tt.IdMedico,
                            tt.DiaSemana,
                            tt.HoraInicio,
                            tt.HoraFin,
                            m.Id AS MedicoId,
                            m.Nombre AS NombreMedico,
                            m.Apellido AS ApellidoMedico
                        FROM TurnoTrabajo tt
                        INNER JOIN Medico m ON tt.IdMedico = m.Id
                    ");

                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        TurnoTrabajo t = new TurnoTrabajo
                        {
                            Id = (int)datos.Lector["Id"],
                            IdMedico = (int)datos.Lector["IdMedico"],
                            //DiaSemana = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), datos.Lector["DiaSemana"].ToString()),
                            DiaSemanaTexto = datos.Lector["DiaSemana"].ToString(),
                            HoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                            HoraFin = (TimeSpan)datos.Lector["HoraFin"],

                            Medico = new Medico
                            {
                                Id = (int)datos.Lector["MedicoId"],
                                Nombre = datos.Lector["NombreMedico"].ToString(),
                                Apellido = datos.Lector["ApellidoMedico"].ToString()
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


        public List<TurnoTrabajo> ListarPorMedico(int idMedico)
        {
            List<TurnoTrabajo> lista = new List<TurnoTrabajo>();

            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
                    SELECT 
                        Id,
                        IdMedico,
                        DiaSemana,
                        HoraInicio,
                        HoraFin
                    FROM TurnoTrabajo
                    WHERE IdMedico = @id
                    ORDER BY DiaSemana, HoraInicio
                ");

                datos.SetearParametro("@id", idMedico);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new TurnoTrabajo
                    {
                        Id = (int)datos.Lector["Id"],
                        IdMedico = (int)datos.Lector["IdMedico"],
                        //DiaSemana = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), datos.Lector["DiaSemana"].ToString()),
                        DiaSemanaTexto = datos.Lector["DiaSemana"].ToString(),
                        HoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                        HoraFin = (TimeSpan)datos.Lector["HoraFin"]
                    });
                }
            }

            return lista;
        }



        public void Agregar(TurnoTrabajo turno)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta(@"
                        INSERT INTO TurnoTrabajo (IdMedico, DiaSemana, HoraInicio, HoraFin)
                        VALUES (@IdMedico, @DiaSemana, @HoraInicio, @HoraFin)
                    ");

                    datos.SetearParametro("@IdMedico", turno.IdMedico);
                    datos.SetearParametro("@DiaSemana", turno.DiaSemanaTexto);
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
                    datos.SetearConsulta(@"
                        UPDATE TurnoTrabajo 
                        SET 
                            IdMedico=@IdMedico, 
                            DiaSemana=@DiaSemana, 
                            HoraInicio=@HoraInicio, 
                            HoraFin=@HoraFin 
                        WHERE Id=@Id
                    ");

                    datos.SetearParametro("@IdMedico", turno.IdMedico);
                    datos.SetearParametro("@DiaSemana", turno.DiaSemanaTexto);
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
                    datos.SetearConsulta("DELETE FROM TurnoTrabajo WHERE Id=@Id");
                    datos.SetearParametro("@Id", id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar turno de trabajo: " + ex.Message);
                }
            }
        }



        public TurnoTrabajo ObtenerHorario(int idMedico, string diaSemana)
        {
            SetearConsulta("SELECT HoraInicio, HoraFin FROM TurnoTrabajo WHERE IdMedico=@id AND DiaSemana=@dia");
            SetearParametro("@id", idMedico);
            SetearParametro("@dia", diaSemana);
            EjecutarLectura();

            if (Lector.Read())
            {
                var turno = new TurnoTrabajo
                {
                    HoraInicio = (TimeSpan)Lector["HoraInicio"],
                    HoraFin = (TimeSpan)Lector["HoraFin"]
                };

                CerrarConexion();
                return turno;
            }

            CerrarConexion();
            return null;
        }

        public void EliminarPorMedico(int idMedico)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("DELETE FROM TurnoTrabajo WHERE IdMedico=@id");
                datos.SetearParametro("@id", idMedico);
                datos.EjecutarAccion();
            }
        }
    }
}