using Messenger.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationContext db;
        public ChatController(ApplicationContext context)
        {
            db = context;
        }
        [Route("chatter")]
        public IActionResult Index()
        {
            ViewBag.Name = User.Identity.Name;
            var group = db.Users.Include(g => g.Group).FirstOrDefault(x => x.Nickname == User.Identity.Name).Group;
            if (group == null)
            {
                ViewBag.Group = "";
            }
            else
            {
                ViewBag.Group = group.Name;
            }
            return View(db.Groups.Include(g => g.Users));
        }

    }
}
