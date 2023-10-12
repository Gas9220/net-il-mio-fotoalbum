using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class MessageController : Controller
    {

        private PhotoContext _context;

        public MessageController(PhotoContext db)
        {
            _context = db;
        }

        public IActionResult Index()
        {

            List<Message> messages = _context.Messages.ToList();

            return View(messages);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
