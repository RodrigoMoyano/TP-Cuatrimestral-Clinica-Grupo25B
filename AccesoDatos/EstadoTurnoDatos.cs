using System;
using System.Collections.Generic;
using Dominio;

namespace AccesoDatos
{
    public class EstadoTurnoDatos
    {
        public List<EstadoTurno> Listar()
        {
            List<EstadoTurno> lista = new List<EstadoTurno>();
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("SELECT Id, Descripcion FROM EstadoTurnos");
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        EstadoTurno e = new EstadoTurno
                        {
                            Id = (int)datos.Lector["Id"],
                            Descripcion = datos.Lector["Descripcion"].ToString()
                        };
                        lista.Add(e);
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar estados de turno: " + ex.Message);
                }
            }
        }

        public void Agregar(EstadoTurno estado)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("INSERT INTO EstadoTurnos (Descripcion) VALUES (@Descripcion)");
                    datos.SetearParametro("@Descripcion", estado.Descripcion);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar estado de turno: " + ex.Message);
                }
            }
        }

        public void Modificar(EstadoTurno estado)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("UPDATE EstadoTurnos SET Descripcion = @Descripcion WHERE Id = @Id");
                    datos.SetearParametro("@Descripcion", estado.Descripcion);
                    datos.SetearParametro("@Id", estado.Id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar estado de turno: " + ex.Message);
                }
            }
        }

        public void Eliminar(int id)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("DELETE FROM EstadoTurnos WHERE Id = @Id");
                    datos.SetearParametro("@Id", id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar estado de turno: " + ex.Message);
                }
            }
        }
    }
}