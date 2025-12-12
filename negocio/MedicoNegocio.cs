using System;
using System.Collections.Generic;
using System.Linq;
using Dominio;

namespace Negocio
{
    public class MedicoNegocio
    {
        // =============================================================
        // LISTAR SOLO ACTIVOS
        // =============================================================
        public List<Medico> Listar()
        {
            List<Medico> lista = new List<Medico>();

            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
                    SELECT Id, Nombre, Apellido, Matricula, Telefono, Email, IdUsuario, Activo
                    FROM Medico
                    WHERE Activo = 1");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico aux = MapearMedico(datos);
                    aux.Especialidad = ObtenerEspecialidadesDeMedico(aux.Id);

                    TurnoTrabajoNegocio ttNegocio = new TurnoTrabajoNegocio();
                    aux.TurnosTrabajo = ttNegocio.ListarPorMedico(aux.Id);

                    lista.Add(aux);
                }
            }

            return lista;
        }

        // =============================================================
        // LISTAR SOLO INACTIVOS
        // =============================================================
        public List<Medico> ListarInactivos()
        {
            List<Medico> lista = new List<Medico>();

            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
                    SELECT Id, Nombre, Apellido, Matricula, Telefono, Email, IdUsuario, Activo
                    FROM Medico
                    WHERE Activo = 0");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico aux = MapearMedico(datos);
                    aux.Especialidad = ObtenerEspecialidadesDeMedico(aux.Id);
                    lista.Add(aux);
                }
            }

            return lista;
        }

        // =============================================================
        // LISTAR ACTIVOS + INACTIVOS
        // =============================================================
        public List<Medico> ListarTodos()
        {
            List<Medico> lista = new List<Medico>();

            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
                    SELECT Id, Nombre, Apellido, Matricula, Telefono, Email, IdUsuario, Activo
                    FROM Medico");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico aux = MapearMedico(datos);
                    aux.Especialidad = ObtenerEspecialidadesDeMedico(aux.Id);
                    lista.Add(aux);
                }
            }

            return lista;
        }

        // =============================================================
        // LISTAR POR ESPECIALIDAD (usado en PedirTurno y AgregarTurno)
        // =============================================================
        public List<Medico> ListarPorEspecialidad(int idEspecialidad)
        {
            List<Medico> lista = new List<Medico>();

            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
                    SELECT M.Id, M.Nombre, M.Apellido, M.Activo
                    FROM Medico M
                    INNER JOIN MedicoEspecialidad ME ON M.Id = ME.IdMedico
                    WHERE ME.IdEspecialidad = @id AND M.Activo = 1");

                datos.SetearParametro("@id", idEspecialidad);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Medico
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Activo = (bool)datos.Lector["Activo"]
                    });
                }
            }

            return lista;
        }

        // =============================================================
        // BUSCAR POR ID
        // =============================================================
        public Medico BuscarPorId(int idMedico)
        {
            Medico medico = null;

            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
                    SELECT Id, Nombre, Apellido, Matricula, Telefono, Email, IdUsuario, Activo
                    FROM Medico
                    WHERE Id = @Id");

                datos.SetearParametro("@Id", idMedico);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    medico = MapearMedico(datos);
                }
            }

            if (medico != null)
            {
                medico.Especialidad = ObtenerEspecialidadesDeMedico(medico.Id);

                TurnoTrabajoNegocio ttNegocio = new TurnoTrabajoNegocio();
                medico.TurnosTrabajo = ttNegocio.ListarPorMedico(medico.Id);
            }

            return medico;
        }

        // =============================================================
        // MAPEO COMÚN DE MÉDICO
        // =============================================================
        private Medico MapearMedico(Datos datos)
        {
            return new Medico
            {
                Id = (int)datos.Lector["Id"],
                Nombre = datos.Lector["Nombre"].ToString(),
                Apellido = datos.Lector["Apellido"].ToString(),
                Matricula = datos.Lector["Matricula"].ToString(),
                Telefono = datos.Lector["Telefono"].ToString(),
                Email = datos.Lector["Email"].ToString(),
                IdUsuario = datos.Lector["IdUsuario"] != DBNull.Value
                                ? Convert.ToInt32(datos.Lector["IdUsuario"])
                                : 0,
                Activo = datos.Lector["Activo"] != DBNull.Value
                                ? Convert.ToBoolean(datos.Lector["Activo"])
                                : true
            };
        }

        // =============================================================
        // AGREGAR MÉDICO
        // =============================================================
        public int Agregar(Medico nuevo)
        {
            int idMedico = 0;

            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
                    INSERT INTO Medico (Nombre, Apellido, Matricula, Telefono, Email, IdUsuario, Activo)
                    OUTPUT INSERTED.Id
                    VALUES (@Nombre, @Apellido, @Matricula, @Telefono, @Email, @IdUsuario, 1)");

                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Apellido", nuevo.Apellido);
                datos.SetearParametro("@Matricula", nuevo.Matricula);
                datos.SetearParametro("@Telefono", nuevo.Telefono);
                datos.SetearParametro("@Email", nuevo.Email);
                datos.SetearParametro("@IdUsuario", nuevo.IdUsuario);

                idMedico = datos.EjecutarAccionEscalar();

                // Insertar especialidades
                if (nuevo.Especialidad != null)
                {
                    foreach (var esp in nuevo.Especialidad)
                    {
                        datos.SetearConsulta("INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (@IdMedico, @IdEspecialidad)");
                        datos.SetearParametro("@IdMedico", idMedico);
                        datos.SetearParametro("@IdEspecialidad", esp.Id);
                        datos.EjecutarAccion();
                    }
                }

                // Insertar turnos de trabajo
                if (nuevo.TurnosTrabajo != null)
                {
                    TurnoTrabajoNegocio ttNegocio = new TurnoTrabajoNegocio();
                    foreach (var tt in nuevo.TurnosTrabajo)
                    {
                        tt.IdMedico = idMedico;
                        ttNegocio.Agregar(tt);
                    }
                }
            }

            return idMedico;
        }

        // =============================================================
        // MODIFICAR MÉDICO
        // =============================================================
        public void Modificar(Medico modificado)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
                    UPDATE Medico 
                    SET Nombre=@Nombre, Apellido=@Apellido, Matricula=@Matricula, 
                        Telefono=@Telefono, Email=@Email
                    WHERE Id=@Id");

                datos.SetearParametro("@Id", modificado.Id);
                datos.SetearParametro("@Nombre", modificado.Nombre);
                datos.SetearParametro("@Apellido", modificado.Apellido);
                datos.SetearParametro("@Matricula", modificado.Matricula);
                datos.SetearParametro("@Telefono", modificado.Telefono);
                datos.SetearParametro("@Email", modificado.Email);
                datos.EjecutarAccion();

                // Borrar especialidades
                datos.SetearConsulta("DELETE FROM MedicoEspecialidad WHERE IdMedico=@IdMedico");
                datos.SetearParametro("@IdMedico", modificado.Id);
                datos.EjecutarAccion();

                // Volver a insertar
                if (modificado.Especialidad != null)
                {
                    foreach (var esp in modificado.Especialidad)
                    {
                        datos.SetearConsulta("INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (@IdMedico, @IdEspecialidad)");
                        datos.SetearParametro("@IdMedico", modificado.Id);
                        datos.SetearParametro("@IdEspecialidad", esp.Id);
                        datos.EjecutarAccion();
                    }
                }
            }
        }

        // =============================================================
        // ELIMINAR (BAJA LÓGICA)
        // =============================================================
        public void Eliminar(int idMedico)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("UPDATE Medico SET Activo = 0 WHERE Id = @Id");
                datos.SetearParametro("@Id", idMedico);
                datos.EjecutarAccion();
            }
        }

        // =============================================================
        // OBTENER ESPECIALIDADES DEL MÉDICO
        // =============================================================
        public List<Especialidad> ObtenerEspecialidadesDeMedico(int idMedico)
        {
            List<Especialidad> lista = new List<Especialidad>();

            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(@"
                    SELECT e.Id, e.Descripcion
                    FROM MedicoEspecialidad me
                    INNER JOIN Especialidad e ON e.Id = me.IdEspecialidad
                    WHERE me.IdMedico = @id");

                datos.SetearParametro("@id", idMedico);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Especialidad
                    {
                        Id = (int)datos.Lector["Id"],
                        Descripcion = datos.Lector["Descripcion"].ToString()
                    });
                }
            }

            return lista;
        }
        //cambia solo estado en la BD
        public void CambiarEstado(int idMedico, bool activo)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("UPDATE Medico SET Activo = @Activo WHERE Id = @Id");
                datos.SetearParametro("@Activo", activo);
                datos.SetearParametro("@Id", idMedico);
                datos.EjecutarAccion();
            }
        }

        public bool ExisteEmail(string email)
        {
            using (Datos datos = new Datos())
            {
                try
                {
                    datos.SetearConsulta("SELECT COUNT(*) FROM Medico WHERE Email = @Email");
                    datos.SetearParametro("@Email", email);

                    int count = datos.EjecutarAccionEscalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al verificar email existente: " + ex.Message);
                }
            }
        }

        public bool ExisteMatricula(string matricula)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta("SELECT COUNT(*) FROM Medico WHERE Matricula = @Matricula");
                datos.SetearParametro("@Matricula", matricula);

                int count = datos.EjecutarAccionEscalar();
                return count > 0;
            }
        }
        public bool ExisteEmail(string email, int idMedicoActual)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(
                    "SELECT COUNT(*) FROM Medico WHERE Email = @Email AND Id <> @Id");
                datos.SetearParametro("@Email", email);
                datos.SetearParametro("@Id", idMedicoActual);

                int count = datos.EjecutarAccionEscalar();
                return count > 0;
            }
        }

        public bool ExisteMatricula(string matricula, int idMedicoActual)
        {
            using (Datos datos = new Datos())
            {
                datos.SetearConsulta(
                    "SELECT COUNT(*) FROM Medico WHERE Matricula = @Matricula AND Id <> @Id");
                datos.SetearParametro("@Matricula", matricula);
                datos.SetearParametro("@Id", idMedicoActual);

                int count = datos.EjecutarAccionEscalar();
                return count > 0;
            }
        }

    }
}