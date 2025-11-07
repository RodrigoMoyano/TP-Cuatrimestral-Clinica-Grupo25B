using System;
using System.Collections.Generic;
using Dominio;

namespace AccesoDatos
{
    public class RolDatos
    {
        public List<Rol> Listar()
        {
            List<Rol> lista = new List<Rol>();
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("SELECT Id, Descripcion FROM Rol");
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Rol r = new Rol
                        {
                            Id = (int)datos.Lector["Id"],
                            Descripcion = datos.Lector["Descripcion"].ToString()
                        };
                        lista.Add(r);
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar roles: " + ex.Message);
                }
            }
        }

        public void Agregar(Rol rol)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("INSERT INTO Rol (Descripcion) VALUES (@Descripcion)");
                    datos.SetearParametro("@Descripcion", rol.Descripcion);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar rol: " + ex.Message);
                }
            }
        }

        public void Modificar(Rol rol)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("UPDATE Rol SET Descripcion = @Descripcion WHERE Id = @Id");
                    datos.SetearParametro("@Descripcion", rol.Descripcion);
                    datos.SetearParametro("@Id", rol.Id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar rol: " + ex.Message);
                }
            }
        }

        public void Eliminar(int id)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("DELETE FROM Rol WHERE Id = @Id");
                    datos.SetearParametro("@Id", id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar rol: " + ex.Message);
                }
            }
        }
    }
}