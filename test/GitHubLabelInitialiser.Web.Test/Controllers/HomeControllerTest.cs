using System.Collections.Generic;
using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Controllers;
using GitHubLabelInitialiser.Web.Helpers;
using GitHubLabelInitialiser.Web.Models;
using Moq;
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

			Assert.That(model.ClientId, Is.EqualTo("d84a8626442322ed91d0"));
		}

		[Test]
		public void Index_WhenCalledWithNoParameters_ThenReturnViewResultWithRedirectUriSet()
		{
			var controller = CreateController();

			var result = controller.Index();
			var model = (HomeIndexViewModel)result.Model;

			Assert.That(model.RedirectUri.ToString(), Is.EqualTo("http://localhost:50038/callback/github"));
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

		private static HomeController CreateController(IConfig config = null)
		{
			return new HomeController(config ?? MockConfig());
		}

		private static IConfig MockConfig()
		{
			var config = new Mock<IConfig>();

			config.Setup(m => m.GitHubClientId()).Returns("d84a8626442322ed91d0");
			config.Setup(m => m.GitHubRedirectUrl()).Returns("http://localhost:50038/callback/github");

			return config.Object;
		}
	}
}
