using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Cobertura
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // "Particular" o "Obra Social"
        public string NombreObraSocial { get; set; } // solo si aplica
        public string Plan { get; set; } // opcional
    }
}
