using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {

        private PhotoContext _context;

        public MessagesController(PhotoContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult CreateMessage([FromBody] Message message)
        {
            Console.Write(message.Email);
            if (message != null)
            {
                _context.Messages.Add(message);
                _context.SaveChanges();

                return Ok("Success");
            }
            return BadRequest(new { Message = "Unable to send message" });

        }
    }
}
