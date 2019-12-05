using Autenticacao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticacao.Repositorie
{
    public class PersonRepository : IPersonRepository
    {

        private readonly List<Person> _storage;

        public PersonRepository()
        {
            _storage = new List<Person>();
        }
        public void Adicionar(Person person)
        {
            _storage.Add(person);
        }

        public void Alterar(Person person)
        {
            var index = _storage.FindIndex(0, 1, x => x.Codigo == person.Codigo);
            _storage[index] = person;
        }

        public IEnumerable<Person> ListPersons()
        {
            return _storage;
        }

        public Person GetById(Guid id)
        {
            return _storage.FirstOrDefault(x => x.Codigo == id);
        }

        public IEnumerable<Person> GetByUF(string uf)
        {

            var persons = new List<Person>();
            //encontrei algumas dificuldades para trabalhar os metodos de lista entáo fiz manualmente
            //nao estava conseguindo retornar uma lista das pessoas que continham a uf pesquisada
            foreach (Person p in _storage)
            {
                if (p.UF == uf)
                {
                    persons.Add(p);

                }
            }

            return persons;

        }

        public void Remover(Person person)
        {
            _storage.Remove(person);
        }


    }
}
