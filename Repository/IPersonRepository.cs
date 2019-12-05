using Autenticacao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticacao.Repositorie
{
    public interface IPersonRepository
    {
        void Adicionar(Person person);

        void Alterar(Person person);

        IEnumerable<Person> ListPersons();

        Person GetById(Guid id);

        IEnumerable<Person> GetByUF(string uf);

        void Remover(Person person);
    }
}
