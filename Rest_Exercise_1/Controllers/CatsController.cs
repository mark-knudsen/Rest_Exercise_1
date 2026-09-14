using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rest_Exercise_1.Models;
using Rest_Exercise_1.Repositories;

namespace Rest_Exercise_1.Controllers
{
    [EnableCors("MyPolicy")] // ADD CORS policy to the controller
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private readonly ICatRepository _repository;

        // Dependency Injection af Singleton-repositoryet
        public CatsController(ICatRepository repository)
        {
            _repository = repository;
        }


        // GET: api/cats
        [DisableCors]
        [HttpGet]
        //[EnableCors("AllowAll")]  // Enable CORS for this endpoint
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Cat>> Get()
        {
            return Ok(_repository.GetAll());
        }

        // GET api/cats/1
        [DisableCors]
        [HttpGet("{id}")]

        public ActionResult<Cat> Get(int id)
        {
            Cat? cat = _repository.GetById(id);
            if (cat == null)
            {
                return NotFound($"Kat med id {id} blev ikke fundet.");
            }
            return Ok(cat);
        }

        // POST api/cats
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Cat> Post([FromBody] Cat newCat)
        {
            if (newCat == null)
            {
                return BadRequest();
            }
            Cat createdCat = _repository.Add(newCat);
            return CreatedAtAction(nameof(Get), new { id = createdCat.Id }, createdCat);
        }

        // PUT api/cats/1
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Cat> Put(int id, [FromBody] Cat updates)
        {
            Cat? updatedCat = _repository.Update(id, updates);
            if (updatedCat == null)
            {
                return NotFound($"Kat med id {id} blev ikke fundet.");
            }
            return Ok(updatedCat);
        }

        // DELETE api/cats/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Cat> Delete(int id)
        {
            Cat? deletedCat = _repository.Delete(id);
            if (deletedCat == null)
            {
                return NotFound($"Kat med id {id} blev ikke fundet.");
            }
            return Ok(deletedCat);
        }

    }
}
