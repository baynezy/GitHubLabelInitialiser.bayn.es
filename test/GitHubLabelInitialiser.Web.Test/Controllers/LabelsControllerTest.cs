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
	class LabelsControllerTest
	{
		[Test]
		public void LabelsController_ExtendsController()
		{
			var controller = CreateController();

			Assert.That(controller, Is.InstanceOf<Controller>());
		}

		[Test]
		public void Index_WhenCalled_ThenGetTokenFromUserSession()
		{
			var user = CreateMockUser();
			var controller = CreateController();

			controller.Index(user.Object);

			user.Verify(f => f.GitHubAccessToken, Times.Once);
		}

		[Test]
		public void Index_WhenAccessTokenIsNull_ThenReturnRedirectResult()
		{
			var user = new Mock<IUser>();
			var controller = CreateController();

			var result = controller.Index(user.Object) as RedirectResult;

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void Index_WhenAccessTokenIsNull_ThenRedirectToHome()
		{
			var user = new Mock<IUser>();
			var controller = CreateController();

			var result = (RedirectResult) controller.Index(user.Object);

			Assert.That(result.Url, Is.EqualTo("/"));
			Assert.That(result.Permanent, Is.False);
		}

		private static Mock<IUser> CreateMockUser()
		{
			var user = new Mock<IUser>();
			user.SetupGet(p => p.GitHubAccessToken).Returns(new GitHubAccessToken
				{
					AccessToken = "some-token",
					Scope = new List<GitHubScope>
						{
							GitHubScope.PublicRepo
						},
					Type = GitHubTokenType.Bearer
				});

			return user;
		}

		private static LabelsController CreateController()
		{
			return new LabelsController();
		}
	}
}
