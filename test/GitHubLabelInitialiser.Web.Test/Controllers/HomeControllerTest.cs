using System.Collections.Generic;
using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Controllers;
using GitHubLabelInitialiser.Web.Models;
using NUnit.Framework;

namespace GitHubLabelInitialiser.Web.Test.Controllers
{
	[TestFixture]
	class HomeControllerTest
	{
		[Test]
		public void HomeController_Extends_Controller()
		{
			var controller = CreateController();

			Assert.That(controller, Is.InstanceOf<Controller>());
		}

		[Test]
		public void Index_WhenCalledWithNoParameters_ThenReturnViewResult()
		{
			var controller = CreateController();

			var result = controller.Index();

			Assert.That(result, Is.InstanceOf<ViewResult>());
		}

		[Test]
		public void Index_WhenCalledWithNoParameters_ThenReturnViewResultWithCorrectViewModel()
		{
			var controller = CreateController();

			var result = controller.Index();

			Assert.That(result.Model, Is.InstanceOf<HomeIndexViewModel>());
		}

		[Test]
		public void Index_WhenCalledWithNoParameters_ThenReturnViewResultWithClientIdSet()
		{
			var controller = CreateController();

			var result = controller.Index();
			var model = (HomeIndexViewModel)result.Model;

			Assert.That(model.ClientId, Is.EqualTo("qwerty"));
		}

		[Test]
		public void Index_WhenCalledWithNoParameters_ThenReturnViewResultWithRedirectUriSet()
		{
			var controller = CreateController();

			var result = controller.Index();
			var model = (HomeIndexViewModel)result.Model;

			Assert.That(model.RedirectUri.ToString(), Is.EqualTo("http://localhost/callback/github/"));
		}

		[Test]
		public void Index_WhenCalledWithNoParameters_ThenReturnViewResultWithScopesSet()
		{
			var expectedScopes = new List<string> {"public_repo"};
			var controller = CreateController();

			var result = controller.Index();
			var model = (HomeIndexViewModel)result.Model;

			Assert.That(model.Scopes, Is.EqualTo(expectedScopes));
		}

		private static HomeController CreateController()
		{
			return new HomeController();
		}
	}
}
