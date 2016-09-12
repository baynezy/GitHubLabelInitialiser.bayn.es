using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using GitHubLabelInitialiser.Models;
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
		public async Task Index_WhenCalled_ThenGetTokenFromUserSession()
		{
			var user = CreateMockUser();
			var controller = CreateController();

			await controller.Index(user.Object);

			user.Verify(f => f.GitHubAccessToken, Times.AtLeast(1));
		}

		[Test]
		public async Task Index_WhenAccessTokenIsNull_ThenReturnRedirectResult()
		{
			var user = CreateMockUser();
			user.SetupGet(p => p.GitHubAccessToken).Returns((Func<GitHubAccessToken>) null);
			var controller = CreateController();

			var result = await controller.Index(user.Object) as RedirectToRouteResult;

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public async Task Index_WhenAccessTokenIsNull_ThenRedirectToHome()
		{
			var user = CreateMockUser();
			user.SetupGet(p => p.GitHubAccessToken).Returns((Func<GitHubAccessToken>) null);
			var controller = CreateController();

			var result = (RedirectToRouteResult) await controller.Index(user.Object);

			Assert.That(result.RouteValues["controller"], Is.EqualTo("home"));
			Assert.That(result.RouteValues["action"], Is.EqualTo("index"));
			Assert.That(result.Permanent, Is.False);
		}

		[Test]
		public async Task Index_WhenAccessTokenIsNotNull_ThenReturnViewResult()
		{
			var user = CreateMockUser();
			var controller = CreateController();

			var result = await controller.Index(user.Object) as ViewResult;

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public async Task Index_WhenAccessTokenIsNotNull_ThenReturnChooseRepositoryViewModel()
		{
			var user = CreateMockUser();
			var controller = CreateController();

			var result = (ViewResult)await controller.Index(user.Object);

			Assert.That(result.Model, Is.InstanceOf<ChooseRepositoryViewModel>());
		}

		[Test]
		public async Task Index_WhenAccessTokenIsNotNull_ThenRetieveRepositoriesForUser()
		{
			var user = CreateMockUser();
			var repoManager = new Mock<IRepositoryManager>();
			var repoFactory = new Mock<IRepositoryManagerFactory>();
			repoFactory.Setup(f => f.Create(It.IsAny<GitHubAccessToken>())).Returns(repoManager.Object);
			var controller = CreateController(repoFactory.Object);

			await controller.Index(user.Object);

			repoManager.Verify(f => f.GetAllForAuthenticatedUser(), Times.Once);
		}

		[Test]
		public async Task Index_WhenAccessTokenIsNotNull_ThenAddRepositoriesToViewModel()
		{
			var user = CreateMockUser();
			var expectedRepos = new List<GitHubRepository>
				{
					new GitHubRepository
						{
							Name = "some-repo"
						}
				};
			var repoManager = new Mock<IRepositoryManager>();
			repoManager.Setup(m => m.GetAllForAuthenticatedUser()).ReturnsAsync(expectedRepos);
			var repoFactory = new Mock<IRepositoryManagerFactory>();
			repoFactory.Setup(m => m.Create(It.IsAny<GitHubAccessToken>())).Returns(repoManager.Object);
			var controller = CreateController(repoFactory.Object);

			var result = (ViewResult)await controller.Index(user.Object);
			var model = (ChooseRepositoryViewModel) result.Model;

			Assert.That(model.Repositories, Is.EqualTo(expectedRepos));
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

		private static LabelsController CreateController(IRepositoryManagerFactory repositoryManagerFactory = null)
		{
			var manager = new Mock<IRepositoryManager>();
			var factory = new Mock<IRepositoryManagerFactory>();
			factory.Setup(f => f.Create(It.IsAny<GitHubAccessToken>())).Returns(manager.Object);
			return new LabelsController(repositoryManagerFactory ?? factory.Object);
		}
	}
}
