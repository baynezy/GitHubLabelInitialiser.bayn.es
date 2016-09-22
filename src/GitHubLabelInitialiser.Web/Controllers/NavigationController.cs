using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Controllers
{
	public class NavigationController : Controller
	{
		private readonly IMeta _meta;

		public NavigationController(IMeta meta)
		{
			_meta = meta;
		}

		public ViewResult Footer()
		{
			var version = _meta.AppVersion();
			return View(new NavigationFooterViewModel { AppVersion = version });
		}
	}
}