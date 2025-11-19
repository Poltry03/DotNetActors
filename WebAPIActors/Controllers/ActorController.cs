using Microsoft.AspNetCore.Mvc;
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
            return DatabaseHelper.GetAllActors();
        }

        [HttpGet("{id}", Name = "GetOneActor")]
        public ActionResult<Actor> GetActor(int id)
        {
            if (id <= 0)
                return BadRequest("Id non valido. Deve essere > 0");
            
            var actor = DatabaseHelper.GetActorById(id);

            if(actor == null)
                return NotFound();

            return actor;
        }
        [HttpGet("research", Name ="FindActor")]
        public ActionResult<List<Actor>> FindActor(string research)
        {
            if (research == null)
                return BadRequest("Inserisci un dato");

            var actors = DatabaseHelper.FindActor(research);

            if(actors== null || actors.Count == 0)
                return NotFound();
            return actors;
        }
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

            return DatabaseHelper.Insert(actor);
        }
    }
}
