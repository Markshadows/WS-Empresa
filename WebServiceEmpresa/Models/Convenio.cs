using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceEmpresa.Models
{
    public class Convenio
    {
        public int ConvenioId { get; set; }
        public int Vigente { get; set; }
        public int Saldo { get; set; }
        public int Empleado { get; set; }
    }
}