using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autenticacao.Models;
using Autenticacao.Repositorie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autenticacao.Controllers
{
    [Authorize(Policy = "UsuarioAPI")]
    public class PersonController : Controller
    {
        //implementei um pattern de repositorio para encapsular de onde os dados venham
        private readonly IPersonRepository _repositorio;

        public PersonController(IPersonRepository repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        [Route("api/persons")]
        public IActionResult Persons()
        {
            return Ok(_repositorio.ListPersons());
        }

        [HttpGet]
        [Route("api/person/{id}")]

        public IActionResult GetPersonByID(Guid Id)
        {
            var person = _repositorio.GetById(Id);
            if (person == null)
                return NotFound();
          
            return Ok(person);
        }

        [HttpGet]
        [Route("api/person/uf/{uf}")]

        public IActionResult GetPersonByUF(string uf)
        {
            var person = _repositorio.GetByUF(uf.ToUpper());
            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpPost]
        [Route("api/person")]

        public IActionResult Adicionar([FromBody]Person person)
        {

            //nao consegui encontrar em um tempo habil uma funcao que validasse a data levando em consideracao que estou recebendo um objeto, entao vou ficar devendo, vamos considerar que sempre passem datas validas

            if (!ValidaCPF(person.CPF))
                return BadRequest("Invalid CPF");

            if((person.CPF == null || person.CPF == "") || (person.Name == null || person.Name == "") || (person.UF == null || person.UF == ""))
                return BadRequest("Fields cannot be blank");

            _repositorio.Adicionar(person);
            return Ok(person);
        }

        [HttpPut]
        [Route("api/person/{Id}")]

        public IActionResult AlterPerson(Guid Id, [FromBody]Person person)
        {
            var oldPerson = _repositorio.GetById(Id);
            if (oldPerson == null)
            {
                return NotFound();
            }

            if (!ValidaCPF(person.CPF))
                return BadRequest("CPF not valid");

            if ((person.CPF == null || person.CPF == "") || (person.Name == null || person.Name == "") || (person.UF == null || person.UF == ""))
                return BadRequest("Fields cannot be blank");

            oldPerson.Name = person.Name;
            oldPerson.CPF = person.CPF;
            oldPerson.UF = person.UF;
            oldPerson.DateNasc = person.DateNasc;

            _repositorio.Alterar(oldPerson);
            return Ok(oldPerson);
        }


        [HttpDelete]
        [Route("api/person/{Id}")]

        public IActionResult RemovePerson(Guid Id)
        {
            var oldPerson = _repositorio.GetById(Id);
            if (oldPerson == null)
            {
                return NotFound();
            }
            _repositorio.Remover(oldPerson);
            return Ok(oldPerson);
        }

        private bool ValidaCPF(string cPF)
        {
            string valor = cPF.Replace(".", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                return false;

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(
                  valor[i].ToString());

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }

            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }

    }
}