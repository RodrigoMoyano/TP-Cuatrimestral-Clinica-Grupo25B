using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace AccesoDatos
{
    public class PacienteDatos
    {
        public List<Paciente> ObtenerTodos()
        {
            List<Paciente> lista = new List<Paciente>();

            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("SELECT Id, Nombre, Apellido, Dni, FechaNacimiento, Email, Telefono FROM Paciente");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Paciente p = new Paciente
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Dni = datos.Lector["Dni"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(datos.Lector["FechaNacimiento"]),
                        Email = datos.Lector["Email"].ToString(),
                        Telefono = datos.Lector["Telefono"].ToString()
                    };
                    lista.Add(p);
                }
            }

            return lista;
        }

        public void Agregar(Paciente paciente)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("INSERT INTO Paciente (Nombre, Apellido, Dni, FechaNacimiento, Email, Telefono) " +
                                     "VALUES (@Nombre, @Apellido, @Dni, @FechaNacimiento, @Email, @Telefono)");

                datos.SetearParametro("@Nombre", paciente.Nombre);
                datos.SetearParametro("@Apellido", paciente.Apellido);
                datos.SetearParametro("@Dni", paciente.Dni);
                datos.SetearParametro("@FechaNacimiento", paciente.FechaNacimiento);
                datos.SetearParametro("@Email", paciente.Email);
                datos.SetearParametro("@Telefono", paciente.Telefono);

                datos.EjecutarAccion();
            }
        }

        public void Modificar(Paciente paciente)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("UPDATE Paciente SET Nombre = @Nombre, Apellido = @Apellido, Dni = @Dni, " +
                                     "FechaNacimiento = @FechaNacimiento, Email = @Email, Telefono = @Telefono " +
                                     "WHERE Id = @Id");

                datos.SetearParametro("@Nombre", paciente.Nombre);
                datos.SetearParametro("@Apellido", paciente.Apellido);
                datos.SetearParametro("@Dni", paciente.Dni);
                datos.SetearParametro("@FechaNacimiento", paciente.FechaNacimiento);
                datos.SetearParametro("@Email", paciente.Email);
                datos.SetearParametro("@Telefono", paciente.Telefono);
                datos.SetearParametro("@Id", paciente.Id);

                datos.EjecutarAccion();
            }
        }

        public void Eliminar(int id)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("DELETE FROM Paciente WHERE Id = @Id");
                datos.SetearParametro("@Id", id);
                datos.EjecutarAccion();
            }
        }
    }
}
