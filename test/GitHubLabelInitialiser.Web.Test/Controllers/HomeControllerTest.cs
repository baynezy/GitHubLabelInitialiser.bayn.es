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

		private static HomeController CreateController()
		{
			return new HomeController();
		}
	}
}
