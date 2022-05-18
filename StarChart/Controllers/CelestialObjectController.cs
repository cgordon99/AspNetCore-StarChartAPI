using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x=>x.Id == id);
            if(celestialObject == null) return NotFound();

            var satellites = _context.CelestialObjects.Where(x=> x.OrbitedObjectId == celestialObject.Id).ToList();
            celestialObject.Satellites = satellites;

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(x=> x.Name == name).ToList();
            if (celestialObjects.Count == 0) return NotFound();

            foreach (var result in celestialObjects)
            {
                var satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == result.Id).ToList();
                result.Satellites = satellites;
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach(var result in celestialObjects)
            {
                var satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == result.Id).ToList();
                result.Satellites = satellites;
            }

            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { celestialObject });
        }

        [HttpPut("{int:id}")]
        public IActionResult Update(int id, CelestialObject celestialObjectUpdate)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (celestialObject == null) return NotFound();

            celestialObject.Name = celestialObjectUpdate.Name;
            celestialObject.OrbitalPeriod = celestialObjectUpdate.OrbitalPeriod;
            celestialObject.OrbitedObjectId = celestialObjectUpdate.OrbitedObjectId;

            _context.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (celestialObject == null) return NotFound();

            celestialObject.Name = name;

            _context.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(x => x.Id == id).ToList();
            if (celestialObjects.Count == 0) return NotFound();

            _context.RemoveRange(celestialObjects);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
