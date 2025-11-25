using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;


namespace Negocio
{
    public class MedicoNegocio
    {
        public List<Medico> Listar()
        {
            List<Medico> lista = new List<Medico>();

            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("SELECT Id, Nombre, Apellido, Matricula, IdUsuario FROM Medico");
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Medico aux = new Medico
                        {
                            Id = (int)datos.Lector["Id"],
                            Nombre = datos.Lector["Nombre"].ToString(),
                            Apellido = datos.Lector["Apellido"].ToString(),
                            Matricula = datos.Lector["Matricula"].ToString(),
                            IdUsuario = (int)datos.Lector["IdUsuario"]
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
                    datos.SetearConsulta("INSERT INTO Medico (Nombre, Apellido, Matricula, IdUsuario) VALUES (@Nombre, @Apellido, @Matricula, @IdUsuario)");
                    datos.SetearParametro("@Nombre", nuevo.Nombre);
                    datos.SetearParametro("@Apellido", nuevo.Apellido);
                    datos.SetearParametro("@Matricula", nuevo.Matricula);
                    datos.SetearParametro("@IdUsuario", nuevo.IdUsuario);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar médico: " + ex.Message);
                }
            }
        }

        /* public void Modificar(Medico modificado)
         {
             using (Datos datos = new Datos())
             {
                 try
                 {
                     datos.SetearConsulta("UPDATE Medico SET Nombre=@Nombre, Apellido=@Apellido, Matricula=@Matricula, IdEspecialidad=@IdUsuario WHERE Id=@Id");
                     datos.SetearParametro("@Id", modificado.Id);
                     datos.SetearParametro("@Nombre", modificado.Nombre);
                     datos.SetearParametro("@Apellido", modificado.Apellido);
                     datos.SetearParametro("@Matricula", modificado.Matricula);
                     datos.SetearParametro("@IdUsuario", modificado.IdUsuario);
                     datos.EjecutarAccion();
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("Error al modificar médico: " + ex.Message);
                 }
             }
         }*/

        /*public void Eliminar(int id)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("DELETE FROM Medico WHERE Id=@Id");
                    datos.SetearParametro("@Id", id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar médico: " + ex.Message);
                }
            }
        }*/
        public List<Especialidad> ObtenerEspecialidadesDeMedico(int idMedico)
        {
            using (var datos = new Datos())
            {
                datos.SetearConsulta(@"
            SELECT e.Id, e.Descripcion
            FROM MedicoEspecialidad me
            INNER JOIN Especialidad e ON e.Id = me.IdEspecialidad
            WHERE me.IdMedico = @id");

                datos.SetearParametro("@id", idMedico);
                datos.EjecutarLectura();

                List<Especialidad> lista = new List<Especialidad>();

                while (datos.Lector.Read())
                {
                    lista.Add(new Especialidad
                    {
                        Id = (int)datos.Lector["Id"],
                        Descripcion = datos.Lector["Descripcion"].ToString()
                    });
                }

                return lista;
            }
        }
        public List<Medico> ListarPorEspecialidad(int idEspecialidad)
        {
            List<Medico> lista = new List<Medico>();
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
            SELECT M.Id, M.Nombre, M.Apellido
            FROM Medico M
            INNER JOIN MedicoEspecialidad ME ON M.Id = ME.IdMedico
            WHERE ME.IdEspecialidad = @id");

                datos.SetearParametro("@id", idEspecialidad);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Medico
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString()
                    });
                }
            }
            return lista;
        }

    }
}