using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly PhotoContext _context;

        public PhotosController(PhotoContext context)
        {
            _context = context;
        }

        // GET: api/Photos
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos()
        {
            if (_context.Photos == null)
            {
                return NotFound();
            }
            return await _context.Photos.ToListAsync();
        }

        [HttpGet("byName")]
        public IActionResult GetPizzasByName(string? searchText)
        {
            if (searchText == null)
            {
                return Ok(new { Message = "Empty text" }); ;
            }

            List<Photo> foundedPizzas = _context.Photos.Where(photo => photo.Title.ToLower().Contains(searchText.ToLower())).ToList();

            return Ok(foundedPizzas);
        }

        // GET: api/Photos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Photo>> GetPhoto(int id)
        {
            if (_context.Photos == null)
            {
                return NotFound();
            }
            var photo = await _context.Photos.FindAsync(id);

            if (photo == null)
            {
                return NotFound();
            }

            return photo;
        }
    }
}
