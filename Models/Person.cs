using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticacao.Models
{
    public class Person
    {

        public Person()
        {
            Codigo = Guid.NewGuid();
        }
        public Guid Codigo { get; }
        public string Name { get; set; }

        public string CPF { get; set; }

        public string UF { get; set; }

        public DateTime DateNasc { get; set; }
    }
}
