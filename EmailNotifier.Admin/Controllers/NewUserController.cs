using System.Threading.Tasks;
using System.Web.Mvc;
using EmailNotifier.Data;
using EmailNotifier.Models;
using EmailNotifier.Publisher;

namespace EmailNotifier.Admin.Controllers
{
    public class NewUserController : Controller
    {
        private readonly INewUserRepository _newUserRepository;
        private readonly IEmailPublisher _emailPublisherObject;

        public NewUserController(INewUserRepository newUserRepository, IEmailPublisher emailPublisherObject)
        {
            _emailPublisherObject = emailPublisherObject;
            _newUserRepository = newUserRepository;
        }

        public async Task<ActionResult> Index()
        {
            var users = await _newUserRepository.GetAllUsers();
            return View(users);
        }

        public async Task<ActionResult> Email(string email, string id)
        {
            var newUser = await _newUserRepository.GetNewUser(id);
            newUser.SentMail = true;
            await _newUserRepository.UpdateNewUser(id, newUser);
            await _emailPublisherObject.SendEmail(email);
            return RedirectToAction("Index");
        }

        public ActionResult AddNewUser()
        {
            return View();
        }

        [HttpPost]
        [ActionName("AddNewUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewUser([Bind(Include = "Id,Name,Email,SentMail")] NewUser newUser)
        {
            if (string.IsNullOrEmpty(newUser.Email))
                ModelState.AddModelError(string.Empty, "Please supply an email value");

            if (string.IsNullOrEmpty(newUser.Name))
                ModelState.AddModelError(string.Empty, "Please supply a user name");

            if (!ModelState.IsValid) return View(newUser);

            await _newUserRepository.CreateNewUser(newUser);

            return RedirectToAction("Index");
        }
    }
}