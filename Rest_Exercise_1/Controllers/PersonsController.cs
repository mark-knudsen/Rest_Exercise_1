using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rest_Exercise_1.Models;
using Rest_Exercise_1.Repositories;

namespace Rest_Exercise_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {

        private readonly IPersonRepository _repository;

        // Dependency Injection af Singleton-repositoryet
        public PersonsController(IPersonRepository repository)
        {
            _repository = repository;
        }

        // GET: api/persons
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Person>> Get()
        {
            return Ok(_repository.GetAll());
        }

        // GET api/persons/1
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Person> Get(int id)
        {
            Person? person = _repository.GetById(id);
            if (person == null)
            {
                return NotFound($"Person med id {id} blev ikke fundet.");
            }
            return Ok(person);
        }


        // POST api/cats
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person> Post([FromBody] Person newPerson)
        {
            if (newPerson == null)
            {
                return BadRequest();
            }
            Person createdPerson = _repository.Add(newPerson);
            return CreatedAtAction(nameof(Get), new { id = createdPerson.Id }, createdPerson);
        }

        // PUT api/cats/1
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Person> Put(int id, [FromBody] Person updates)
        {
            Person? updatedCat = _repository.Update(id, updates);
            if (updatedCat == null)
            {
                return NotFound($"Person med id {id} blev ikke fundet.");
            }
            return Ok(updatedCat);
        }

        // DELETE api/cats/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Person> Delete(int id)
        {
            Person? deletedPerson = _repository.Delete(id);
            if (deletedPerson == null)
            {
                return NotFound($"Person med id {id} blev ikke fundet.");
            }
            return Ok(deletedPerson);
        }


    }
}
