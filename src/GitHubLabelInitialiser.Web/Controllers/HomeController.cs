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
			return View(new HomeIndexViewModel { ClientId = "d84a8626442322ed91d0", RedirectUri = new Uri("http://localhost:50038/callback/github"), Scopes = new List<string> { "public_repo" } });
		}
	}
}