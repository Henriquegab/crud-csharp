using API.Entities;
using API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventsDbContext _context;
        public DevEventsController(DevEventsDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var devEvents = _context.DevEvents.Where(d => !d.IsDeleted).ToList();

            return Ok(devEvents);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var devEvent = _context.DevEvents.Include(de => de.Speakers).SingleOrDefault(d => d.id == id);
            
            if(devEvent == null)
            {
                return NotFound();
            }

            return Ok(devEvent);
        }
        [HttpPost]
        public IActionResult Post(DevEvent devEvent)
        {
            _context.DevEvents.Add(devEvent);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = devEvent.id }, devEvent);
        }
        // api/dev-events/1 PUT
        [HttpPut("{id}")]
        public IActionResult Update(int id,DevEvent input)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.id == id);

            if (devEvent == null)
            {
                return NotFound();
            }

            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);

            _context.DevEvents.Update(devEvent);
            _context.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.id == id);

            if (devEvent == null)
            {
                return NotFound();
            }

            devEvent.Delete();
            _context.SaveChanges();

            return NoContent();
        }
        [HttpPost("{id}/speakers")]
        public IActionResult PostSpeaker(int id,DevEventSpeaker speaker)
        {
            speaker.DevEventId = id;

            var devEvent = _context.DevEvents.Any(d => d.id == id);

            if (!devEvent)
            {
                return NotFound();
            }

            _context.DevEventSpeaker.Add(speaker);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
