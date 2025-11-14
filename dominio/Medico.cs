using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Medico : Usuario
    {
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Matricula { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        public List<Especialidad> Especialidad { get; set; }
        public List<TurnoTrabajo> TurnosTrabajo { get; set; }

        public Usuario Usuario { get; set; } // Relación con la cuenta del sistema
    }
}
