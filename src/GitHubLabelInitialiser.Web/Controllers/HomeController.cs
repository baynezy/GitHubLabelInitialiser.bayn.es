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
		private readonly IGitHubStateGenerator _gitHubStateGenerator;

		public HomeController(IConfig config, IGitHubStateGenerator gitHubStateGenerator)
		{
			_config = config;
			_gitHubStateGenerator = gitHubStateGenerator;
		}

		public ViewResult Index()
		{
			var gitHubClientId = _config.GitHubClientId;
			var githubRedirectUrl = _config.GitHubRedirectUrl;
			var state = _gitHubStateGenerator.GenerateState();

			Session["GitHubAuthenticationState"] = state;
			return View(new HomeIndexViewModel { ClientId = gitHubClientId, RedirectUri = new Uri(githubRedirectUrl), Scopes = new List<GitHubScope> { GitHubScope.PublicRepo }, State = state });
		}
	}
}