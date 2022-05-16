using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

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

        [HttpGet("GetById/{id:int}")]
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
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x=> x.Name == name);
            if (celestialObject == null) return NotFound();

            var satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();
            celestialObject.Satellites = satellites;

            return Ok(celestialObject);
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
    }
}
