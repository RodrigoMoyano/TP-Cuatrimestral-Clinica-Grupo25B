using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Negocio
{
    public class CoberturaNegocio
    {
        // 🔹 Listar todas las coberturas (obra social o particular)
        public List<Cobertura> Listar()
        {
            List<Cobertura> lista = new List<Cobertura>();
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("SELECT Id, Tipo, NombreObraSocial, Plan FROM Cobertura");
                    datos.EjecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Cobertura c = new Cobertura();
                        c.Id = (int)datos.Lector["Id"];
                        c.Tipo = datos.Lector["Tipo"].ToString();
                        c.NombreObraSocial = datos.Lector["NombreObraSocial"] != DBNull.Value ? datos.Lector["NombreObraSocial"].ToString() : null;
                        c.Plan = datos.Lector["Plan"] != DBNull.Value ? datos.Lector["Plan"].ToString() : null;

                        lista.Add(c);
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar coberturas: " + ex.Message);
                }
            }
        }

        // 🔹 Agregar una cobertura nueva
        public void Agregar(Cobertura nueva)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("INSERT INTO Cobertura (Tipo, NombreObraSocial, Plan) VALUES (@Tipo, @NombreObraSocial, @Plan)");
                    datos.SetearParametro("@Tipo", nueva.Tipo);
                    datos.SetearParametro("@NombreObraSocial", nueva.NombreObraSocial);
                    datos.SetearParametro("@Plan", nueva.Plan);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar cobertura: " + ex.Message);
                }
            }
        }

        // 🔹 Modificar cobertura existente
        public void Modificar(Cobertura cobertura)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("UPDATE Cobertura SET Tipo = @Tipo, NombreObraSocial = @NombreObraSocial, Plan = @Plan WHERE Id = @Id");
                    datos.SetearParametro("@Tipo", cobertura.Tipo);
                    datos.SetearParametro("@NombreObraSocial", cobertura.NombreObraSocial);
                    datos.SetearParametro("@Plan", cobertura.Plan);
                    datos.SetearParametro("@Id", cobertura.Id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar cobertura: " + ex.Message);
                }
            }
        }

        // 🔹 Eliminar cobertura
        public void Eliminar(int id)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("DELETE FROM Cobertura WHERE Id = @Id");
                    datos.SetearParametro("@Id", id);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar cobertura: " + ex.Message);
                }
            }
        }
    }
}
