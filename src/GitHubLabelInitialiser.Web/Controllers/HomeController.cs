using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Controllers
{
	public class HomeController : Controller
	{
		public ViewResult Index()
		{
			return View(new HomeIndexViewModel());
		}
	}
}