using Dominio;
using System;
using System.Collections.Generic;

namespace AccesoDatos
{
    public class EspecialidadDatos : Datos
    {
        public List<Especialidad> Listar()
        {
            List<Especialidad> lista = new List<Especialidad>();
            try
            {
                SetearConsulta("SELECT IdEspecialidad, Nombre, Descripcion  FROM Especialidad");
                EjecutarLectura();

                while (Lector.Read())
                {
                    Especialidad aux = new Especialidad
                    {
                        IdEspecialidad = (int)Lector["IdEspecialidad"],
                        Nombre = Lector["Nombre"].ToString(),
                        Descripcion = Lector["Descripcion"].ToString()
                    };
                    lista.Add(aux);
                }

                return lista;
            }
            finally { CerrarConexion(); }
        }

        public void Agregar(Especialidad nueva)
        {
            try
            {
                SetearConsulta("INSERT INTO Especialidad (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)");
                SetearParametro("@Nombre", nueva.Nombre);
                SetearParametro("@Descripcion", nueva.Descripcion);
            }
            finally { CerrarConexion(); }
        }

        public void Modificar(Especialidad mod)
        {
            try
            {
                SetearConsulta("UPDATE Especialidad SET Nombre=@Nombre, Descripcion=@Descripcion WHERE IdEspecialidad=@IdEspecialidad");
                SetearParametro("@IdEspecialidad", mod.IdEspecialidad);
                SetearParametro("@Nombre", mod.Nombre);
                SetearParametro("@Descripcion", mod.Descripcion);
                EjecutarAccion();
            }
            finally { CerrarConexion(); }
        }

        public void Eliminar(int id)
        {
            try
            {
                SetearConsulta("DELETE FROM Especialidad WHERE IdEspecialidad=@IdEspecialidad");
                SetearParametro("@IdEspecialidad", id);
                EjecutarAccion();
            }
            finally { CerrarConexion(); }
        }
    }
}