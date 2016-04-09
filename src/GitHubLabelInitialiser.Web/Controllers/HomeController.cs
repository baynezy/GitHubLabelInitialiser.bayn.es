using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Controllers
{
	public class HomeController : Controller
	{
		public ViewResult Index()
		{
			return View(new HomeIndexViewModel{ClientId = "qwerty", RedirectUri = new Uri("http://localhost/callback/github/"), Scopes = new List<string>{"public_repo"}});
		}
	}
}