using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Helpers;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IConfig _config;

		public HomeController(IConfig config)
		{
			_config = config;
		}

		public ViewResult Index()
		{
			var gitHubClientId = _config.GitHubClientId();
			var githubRedirectUrl = _config.GitHubRedirectUrl();
			return View(new HomeIndexViewModel { ClientId = gitHubClientId, RedirectUri = new Uri(githubRedirectUrl), Scopes = new List<string> { "public_repo" } });
		}
	}
}