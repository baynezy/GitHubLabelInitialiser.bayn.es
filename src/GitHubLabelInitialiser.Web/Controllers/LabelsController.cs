using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Controllers
{
	public class LabelsController : Controller
	{
		public ActionResult Index(IUser user)
		{
			var gitHubAccessToken = user.GitHubAccessToken;

			return RedirectToAction("index", "home");
		}
	}
}