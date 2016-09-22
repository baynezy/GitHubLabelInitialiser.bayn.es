using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Controllers;
using GitHubLabelInitialiser.Web.Models;
using Moq;
using NUnit.Framework;

namespace GitHubLabelInitialiser.Web.Test.Controllers
{
	[TestFixture]
	internal class NavigationControllerTest
	{
		[Test]
		public void NavigationController_Extends_Controller()
		{
			var controller = CreateController();

			Assert.That(controller, Is.InstanceOf<Controller>());
		}

		#region Footer

		[Test]
		public void Footer_WhenCalled_ThenReturnsViewResult()
		{
			var controller = CreateController();

			var result = controller.Footer();

			Assert.That(result, Is.InstanceOf<ViewResult>());
		}

		[Test]
		public void Footer_WhenCalled_ThenCallIMetaAppVersion()
		{
			const string version = "1.0.1";
			var meta = new Mock<IMeta>();
			meta.Setup(m => m.AppVersion()).Returns(version);

			var controller = CreateController(meta.Object);

			controller.Footer();

			meta.Verify(f => f.AppVersion(), Times.Once);
		}

		[Test]
		public void Footer_WhenCalled_ThenShouldReturnNavigationFooterViewModel()
		{
			var controller = CreateController();

			var result = controller.Footer();

			Assert.That(result.Model, Is.InstanceOf<NavigationFooterViewModel>());
		}

		[Test]
		public void Footer_WhenCalled_ThenShouldPopulateVersionOnViewModel()
		{
			const string version = "1.0.1";
			var meta = new Mock<IMeta>();
			meta.Setup(m => m.AppVersion()).Returns(version);

			var controller = CreateController(meta.Object);

			var result = controller.Footer();
			var model = (NavigationFooterViewModel)result.Model;

			Assert.That(model.AppVersion, Is.EqualTo(version));
		}

		#endregion

		private static NavigationController CreateController(IMeta meta = null)
		{
			return new NavigationController(meta ?? new Mock<IMeta>().Object);
		}
	}
}
