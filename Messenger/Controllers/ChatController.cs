using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    public class ChatController : Controller
    {
        [Authorize]
        [Route("chatter")]
        public IActionResult Index()
        {
            ViewBag.Name = User.Identity.Name;
            return View();
        }
    }
}
