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
       
        public List<Cobertura> Listar()
        {
            List<Cobertura> lista = new List<Cobertura>();

            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("SELECT Id, Tipo, NombreObraSocial, PlanCobertura FROM Cobertura");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Cobertura
                    {
                        Id = (int)datos.Lector["Id"],
                        Tipo = datos.Lector["Tipo"].ToString(),
                        NombreObraSocial = datos.Lector["NombreObraSocial"].ToString(),
                        PlanCobertura = datos.Lector["PlanCobertura"].ToString()
                    });
                }
            }

            return lista;
        }
    


        // 🔹 Agregar una cobertura nueva
       /* public void Agregar(Cobertura nueva)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("INSERT INTO Cobertura (Tipo, NombreObraSocial, Plan) VALUES (@Tipo, @NombreObraSocial, @PlanCobertura)");
                    datos.SetearParametro("@Tipo", nueva.Tipo);
                    datos.SetearParametro("@NombreObraSocial", nueva.NombreObraSocial);
                    datos.SetearParametro("@PlanCobertura", nueva.PlanCobertura);
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
                    datos.SetearConsulta("UPDATE Cobertura SET Tipo = @Tipo, NombreObraSocial = @NombreObraSocial, PlanCobertura = @Plan WHERE IdCobertura = @IdCobertura");
                    datos.SetearParametro("@Tipo", cobertura.Tipo);
                    datos.SetearParametro("@NombreObraSocial", cobertura.NombreObraSocial);
                    datos.SetearParametro("@PlanCobertura", cobertura.PlanCobertura);
                    datos.SetearParametro("@IdCobertura", cobertura.IdCobertura);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar cobertura: " + ex.Message);
                }
            }
        }

        // 🔹 Eliminar cobertura
        public void Eliminar(int idCobertura)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("DELETE FROM Cobertura WHERE IdCobertura = @IdCobertura");
                    datos.SetearParametro("@IdCobertura", idCobertura);
                    datos.EjecutarAccion();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar cobertura: " + ex.Message);
                }
            }
        }*/
    }
}
