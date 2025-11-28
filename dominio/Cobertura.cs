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
        public string Tipo { get; set; } 
        public string NombreObraSocial { get; set; } 
        public string PlanCobertura { get; set; }

        public string NombreCompleto
        {
            get { return $"{Tipo} - {NombreObraSocial} ({PlanCobertura})"; }
        }

    }
}
