using System;
using System.Collections.Generic;

namespace Dominio
{
    public class Medico
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Matricula { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";

        public List<Especialidad> Especialidad { get; set; }
        public List<TurnoTrabajo> TurnosTrabajo { get; set; }

        public Usuario Usuario { get; set; }
        public bool Activo { get; set; }

        // ==============================
        //  ESPECIALIDADES EN TEXTO
        // ==============================
        public string EspecialidadesTexto
        {
            get
            {
                if (Especialidad == null || Especialidad.Count == 0)
                    return "-";

                // ej: "Clínica, Pediatría, Cardiología"
                return string.Join(", ", Especialidad.ConvertAll(e => e.Descripcion));
            }
        }

        // ==============================
        //  DISPONIBILIDAD EN TEXTO
        // ==============================
        public string DisponibilidadTexto
        {
            get
            {
                if (TurnosTrabajo == null || TurnosTrabajo.Count == 0)
                    return "-";

                var partes = new List<string>();

                foreach (var t in TurnosTrabajo)
                {
                    // Podríamos pasarlo a español con CultureInfo, pero por ahora simple:
                    string dia = t.DiaSemanaTexto;

                    partes.Add($"{dia} {t.HoraInicio:hh\\:mm} - {t.HoraFin:hh\\:mm}");
                }

                // ej: "Monday 08:00 - 12:00, Wednesday 10:00 - 18:00"
                return string.Join("\n", partes);
            }
        }
    }
}