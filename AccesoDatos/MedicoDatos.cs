﻿using Dominio;
using System;
using System.Collections.Generic;

namespace AccesoDatos
{
    public class MedicoDatos
    {
        public List<Medico> Listar()
        {
            List<Medico> lista = new List<Medico>();

            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("SELECT Id, Nombre, Apellido, Matricula, IdEspecialidad FROM Medicos");
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Medico aux = new Medico
                        {
                            Id = (int)datos.Lector["Id"],
                            Nombre = datos.Lector["Nombre"].ToString(),
                            Apellido = datos.Lector["Apellido"].ToString(),
                            Matricula = datos.Lector["Matricula"].ToString(),
                            IdEspecialidad = (int)datos.Lector["IdEspecialidad"]
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
                    datos.SetearConsulta("INSERT INTO Medicos (Nombre, Apellido, Matricula, IdEspecialidad) VALUES (@Nombre, @Apellido, @Matricula, @IdEspecialidad)");
                    datos.SetearParametro("@Nombre", nuevo.Nombre);
                    datos.SetearParametro("@Apellido", nuevo.Apellido);
                    datos.SetearParametro("@Matricula", nuevo.Matricula);
                    datos.SetearParametro("@IdEspecialidad", nuevo.IdEspecialidad);
                    datos.EjecutarAccion();
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
                    datos.SetearConsulta("UPDATE Medicos SET Nombre=@Nombre, Apellido=@Apellido, Matricula=@Matricula, IdEspecialidad=@IdEspecialidad WHERE Id=@Id");
                    datos.SetearParametro("@Id", modificado.Id);
                    datos.SetearParametro("@Nombre", modificado.Nombre);
                    datos.SetearParametro("@Apellido", modificado.Apellido);
                    datos.SetearParametro("@Matricula", modificado.Matricula);
                    datos.SetearParametro("@IdEspecialidad", modificado.IdEspecialidad);
                    datos.EjecutarAccion();
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
                    datos.SetearConsulta("DELETE FROM Medicos WHERE Id=@Id");
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