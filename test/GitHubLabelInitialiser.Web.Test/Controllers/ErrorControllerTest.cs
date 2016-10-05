using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Controllers;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace GitHubLabelInitialiser.Web.Test.Controllers
{
	[TestFixture]
	class ErrorControllerTest
	{
		[Test]
		public void ErrorController_Implements_Controller()
		{
			var controller = CreateController();

			Assert.That(controller, Is.InstanceOf<Controller>());
		}

		[Test]
		public void Index_WhenCalled_ThenReturnViewResult()
		{
			var controller = CreateController();

			var result = controller.Index();

			Assert.That(result, Is.InstanceOf<ViewResult>());
		}

		[Test]
		public void Index_WhenCalled_ThenViewNameIsCorrect()
		{
			var controller = CreateController();

			var result = controller.Index();

			Assert.That(result.ViewName, Is.EqualTo("Error"));
		}

		[Test]
		public void NotFound_WhenCalled_ThenReturnViewResult()
		{
			var controller = CreateController();

			var result = controller.NotFound();

			Assert.That(result, Is.InstanceOf<ViewResult>());
		}

		[Test]
		public void NotFound_WhenCalled_ThenViewNameIsCorrect()
		{
			var controller = CreateController();

			var result = controller.NotFound();

			Assert.That(result.ViewName, Is.EqualTo("NotFound"));
		}

		private static ErrorController CreateController()
		{
			return new ErrorController();
		}
	}
}
