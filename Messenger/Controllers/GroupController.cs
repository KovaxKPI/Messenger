using Messenger.Database;
using Messenger.Models;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    public class GroupController : Controller
    {
        private readonly ApplicationContext db;
        public GroupController(ApplicationContext context)
        {
            db = context;
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Group group)
        {
            db.Groups.Add(group);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Chat");
        }
    }
}
