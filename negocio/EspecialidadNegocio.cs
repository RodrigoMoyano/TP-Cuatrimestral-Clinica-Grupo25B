using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class EspecialidadNegocio : Datos
    {
        public List<Especialidad> Listar()
        {
            List<Especialidad> lista = new List<Especialidad>();

            try
            {
                SetearConsulta("SELECT Id, Descripcion FROM Especialidad");
                EjecutarLectura();

                while (Lector.Read())
                {
                    Especialidad aux = new Especialidad
                    {
                        Id = (int)Lector["Id"],
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
                SetearConsulta("INSERT INTO Especialidad (Descripcion) VALUES (@Descripcion)");
                SetearParametro("@Descripcion", nueva.Descripcion);
                EjecutarAccion();
            }
            finally { CerrarConexion(); }
        }

        public void Modificar(Especialidad mod)
        {
            try
            {
                SetearConsulta("UPDATE Especialidad SET Descripcion=@Descripcion WHERE Id=@Id");
                SetearParametro("@Descripcion", mod.Descripcion);
                SetearParametro("@Id", mod.Id);
                EjecutarAccion();
            }
            finally { CerrarConexion(); }
        }

        public void Eliminar(int id)
        {
            try
            {
                SetearConsulta("DELETE FROM Especialidad WHERE Id=@Id");
                SetearParametro("@Id", id);
                EjecutarAccion();
            }
            finally { CerrarConexion(); }
        }
    }
}