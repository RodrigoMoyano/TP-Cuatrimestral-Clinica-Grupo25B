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
                SetearConsulta("SELECT Id, Nombre FROM Especialidades");
                EjecutarLectura();

                while (Lector.Read())
                {
                    Especialidad aux = new Especialidad
                    {
                        Id = (int)Lector["Id"],
                        Nombre = Lector["Nombre"].ToString()
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
                SetearConsulta("INSERT INTO Especialidades (Nombre) VALUES (@Nombre)");
                SetearParametro("@Nombre", nueva.Nombre);
                EjecutarAccion();
            }
            finally { CerrarConexion(); }
        }

        public void Modificar(Especialidad mod)
        {
            try
            {
                SetearConsulta("UPDATE Especialidades SET Nombre=@Nombre WHERE Id=@Id");
                SetearParametro("@Id", mod.Id);
                SetearParametro("@Nombre", mod.Nombre);
                EjecutarAccion();
            }
            finally { CerrarConexion(); }
        }

        public void Eliminar(int id)
        {
            try
            {
                SetearConsulta("DELETE FROM Especialidades WHERE Id=@Id");
                SetearParametro("@Id", id);
                EjecutarAccion();
            }
            finally { CerrarConexion(); }
        }
    }
}
