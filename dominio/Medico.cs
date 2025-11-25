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
        public int Id{ get; set; }
        public int IdUsuario{ get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Matricula { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";

        public List<Especialidad> Especialidad { get; set; }
        public List<TurnoTrabajo> TurnosTrabajo { get; set; }

        public Usuario Usuario { get; set; }
    }
}
