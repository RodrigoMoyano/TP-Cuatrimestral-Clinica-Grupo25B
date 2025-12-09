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
                SetearConsulta("SELECT Id, Descripcion , Imagen FROM Especialidad");
                EjecutarLectura();

                while (Lector.Read())
                {
                    Especialidad aux = new Especialidad
                    {
                        Id = (int)Lector["Id"],
                        Descripcion = Lector["Descripcion"].ToString(),
                        Imagen = Lector["Imagen"] == DBNull.Value ? null : Lector["Imagen"].ToString()
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
                SetearConsulta("INSERT INTO Especialidad (Descripcion, Imagen) VALUES (@Descripcion, @Imagen)");
                SetearParametro("@Descripcion", nueva.Descripcion);
                SetearParametro("@Imagen", (object)nueva.Imagen ?? DBNull.Value);
                EjecutarAccion();
            }
            finally { CerrarConexion(); }
        }
        public Especialidad BuscarPorId(int id)
        {
            Especialidad esp = null;
            try
            {
                SetearConsulta("SELECT Id, Descripcion, Imagen FROM Especialidad WHERE Id = @Id");
                SetearParametro("@Id", id);
                EjecutarLectura();

                if (Lector.Read())
                {
                    esp = new Especialidad();
                    esp.Id = (int)Lector["Id"];
                    esp.Descripcion = (string)Lector["Descripcion"];
                    esp.Imagen = Lector["Imagen"] != DBNull.Value ? (string)Lector["Imagen"] : "";
                }
            }
            finally { CerrarConexion(); }

            return esp;
        }

        public void Modificar(Especialidad mod)
        {
            try
            {
                SetearConsulta("UPDATE Especialidad SET Descripcion=@Descripcion, Imagen=@Imagen WHERE Id=@Id");
                SetearParametro("@Descripcion", mod.Descripcion);
                SetearParametro("@Imagen", mod.Imagen);
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