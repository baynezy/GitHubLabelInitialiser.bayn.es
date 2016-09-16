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

		[Test]
		public async Task Update_WhenCallingWithUsernameAndRepository_ThenReturnViewResult()
		{
			const string username = "username";
			const string repository = "repository";
			var controller = CreateController();

			var result = await controller.Update(username, repository, CreateMockUser().Object);

			Assert.That(result, Is.InstanceOf<ViewResult>());
		}

		[Test]
		public async Task Update_WhenCallingWithUsernameAndRepository_ThenReturnUpdateLabelViewModel()
		{
			const string username = "username";
			const string repository = "repository";
			var controller = CreateController();

			var result = await controller.Update(username, repository, CreateMockUser().Object);

			var model = result.Model;

			Assert.That(model, Is.InstanceOf<UpdateLabelViewModel>());
		}

		[Test]
		public void Update_WhenCallingWithUsernameAndRepository_ThenReturnCurrentLabels()
		{
			const string username = "username";
			const string repository = "repository";

			var labelManager = new Mock<ILabelManager>();
			labelManager.Setup(m => m.GetLabelsForRepository(username, repository));

			var controller = CreateController(labelManagerFactory: CreateMockLabelManagerFactory(labelManager));

			controller.Update(username, repository, CreateMockUser().Object);

			labelManager.Verify(f => f.GetLabelsForRepository(username, repository), Times.Once);
		}

		[Test]
		public async Task Update_WhenCallingWithUsernameAndRepository_ThenReturnLabelsInViewModel()
		{
			const string username = "username";
			const string repository = "repository";

			var expectedLabels = new List<GitHubLabel>
				{
					new GitHubLabel
						{
							Color = "#ff0000",
							Name = "Bug"
						},
					new GitHubLabel
						{
							Color = "#ffffff",
							Name = "Wont Fix"
						}
				};

			var labelManager = new Mock<ILabelManager>();
			labelManager.Setup(m => m.GetLabelsForRepository(username, repository)).ReturnsAsync(expectedLabels);

			var controller = CreateController(labelManagerFactory: CreateMockLabelManagerFactory(labelManager));

			var result = await controller.Update(username, repository, CreateMockUser().Object);
			var model = (UpdateLabelViewModel) result.Model;

			Assert.That(model.Labels, Is.EqualTo(expectedLabels));
		}

		private static ILabelManagerFactory CreateMockLabelManagerFactory(Mock<ILabelManager> labelManager)
		{
			var factory = new Mock<ILabelManagerFactory>();
			factory.Setup(m => m.Create(It.IsAny<GitHubAccessToken>())).Returns(labelManager.Object);

			return factory.Object;
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

		private static LabelsController CreateController(IRepositoryManagerFactory repositoryManagerFactory = null, ILabelManagerFactory labelManagerFactory = null)
		{
			var mockRepositoryManager = new Mock<IRepositoryManager>();
			var mockRepositoryManagerFactory = new Mock<IRepositoryManagerFactory>();
			mockRepositoryManagerFactory.Setup(f => f.Create(It.IsAny<GitHubAccessToken>())).Returns(mockRepositoryManager.Object);

			var mockLabelManager = new Mock<ILabelManager>();
			return new LabelsController(repositoryManagerFactory ?? mockRepositoryManagerFactory.Object, labelManagerFactory ?? CreateMockLabelManagerFactory(mockLabelManager));
		}
	}
}
