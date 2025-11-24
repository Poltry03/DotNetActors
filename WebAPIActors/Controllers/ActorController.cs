using Microsoft.AspNetCore.Mvc;
using WebAPIActors.Attributes;
using WebAPIActors.Helper;
using WebAPIActors.Models;

namespace WebAPIActors.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActorController : Controller
    {
        [HttpGet(Name = "GetAllActors")]
        public List<Actor> GetAllActors()
        {
            return ActorHelper.GetAllActors();
        }

        [HttpGet("{id}", Name = "GetOneActor")]
        public ActionResult<Actor> GetActor(int id)
        {
            if (id <= 0)
                return BadRequest("Id non valido. Deve essere > 0");

            var actor = ActorHelper.GetActorById(id);

            if (actor == null)
                return NotFound();

            return actor;
        }
        [HttpGet("research", Name = "FindActor")]
        public ActionResult<List<Actor>> FindActor(string research)
        {
            if (research == null)
                return BadRequest("Inserisci un dato");

            var actors = ActorHelper.FindActor(research);

            if (actors == null || actors.Count == 0)
                return NotFound();
            return actors;
        }

        [JwtAuth]
        [HttpPost("", Name = "AddActor")]
        public ActionResult<int> AddActor(Actor actor)
        {
            if (actor == null ||
                string.IsNullOrEmpty(actor.Name) ||
                string.IsNullOrEmpty(actor.Surname) ||
                string.IsNullOrEmpty(actor.ImgUrl) ||
                string.IsNullOrEmpty(actor.Nation))
                return BadRequest("I campi contrasegnati da * sono obbligatori");
            if (actor.FilmCachet <= 0)
                return BadRequest("Il vaolre del cachet non può essere negativo");
            if (actor.FilmQuantity <= 0)
                return BadRequest("Non può aver fatto meno di 0 film");

            return ActorHelper.Insert(actor);
        }

        [JwtAuth]
        [HttpPut("{id}", Name = "ModifyActor")]
        public ActionResult<bool> ModifyActor(int id, Actor actor)
        {
            if (id <= 0)
                return BadRequest("Id non valido");

            var test = ActorHelper.GetActorById(id);
            if (test == null)
                return NotFound();

            bool result = false;

            test.Name = actor.Name;
            test.Surname = actor.Surname;
            test.ImgUrl = actor.ImgUrl;
            test.Nation = actor.Nation;
            test.FilmQuantity = actor.FilmQuantity;
            test.FilmCachet = actor.FilmCachet;

            if (ModelState.IsValid)
            {
                test.Id = id;

                result = ActorHelper.Update(test);
            }

            return result;
        }

        [JwtAuth]
        [HttpDelete("{id}", Name = "DeleteActor")]
        public ActionResult DeleteActor(int id)
        {
            if (id <= 0)
                return BadRequest("Id non valido");

            var test = ActorHelper.GetActorById(id);
            if (test == null)
                return NotFound();

            bool result = ActorHelper.Delete(test.Id);

            if (!result)
                return StatusCode(500, "Riprova dopo");
            return NoContent();
        }
    }
}
