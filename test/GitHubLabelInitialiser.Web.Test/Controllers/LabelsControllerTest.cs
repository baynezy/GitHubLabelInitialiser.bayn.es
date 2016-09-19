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
			var controller = CreateController(repoManager.Object);

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
			var controller = CreateController(repoManager.Object);

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

			var controller = CreateController(labelManager:labelManager.Object);

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

			var controller = CreateController(labelManager:labelManager.Object);

			var result = await controller.Update(username, repository, CreateMockUser().Object);
			var model = (UpdateLabelViewModel) result.Model;

			AssertLabelsMatch(model.Labels, expectedLabels);
		}

		[Test]
		public async Task Initialise_WhenCallingWithValidModel_ThenReturnViewResult()
		{
			var controller = CreateController();
			var viewModel = CreateValidUpdateLabelViewModel();
			const string username = "username";
			const string repository = "repository";

			var result = await controller.Initialise(username, repository, viewModel, CreateMockUser().Object) as ViewResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ViewName, Is.EqualTo(""));
		}

		[Test]
		public async Task Initialise_WhenCallingWithValidModel_ThenReturnCallLabelManagerIntialiseLabels()
		{
			var labelManager = new Mock<ILabelManager>();
			var viewModel = CreateValidUpdateLabelViewModel();
			const string username = "username";
			const string repository = "repository";

			var controller = CreateController(labelManager: labelManager.Object);

			await controller.Initialise(username, repository, viewModel, CreateMockUser().Object);

			labelManager.Verify(f => f.IntialiseLabels(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<GitHubLabel>>()), Times.Once);
		}

		private static UpdateLabelViewModel CreateValidUpdateLabelViewModel()
		{
			return new UpdateLabelViewModel
			{
				Labels = new List<GitHubLabelViewModel>
						{
							new GitHubLabelViewModel
								{
									Color = "000000",
									Name = "Never"
								}
						},
				Username = "username",
				Repository = "repository"
			};
		}

		private static UpdateLabelViewModel CreateInvalidUpdateLabelViewModel()
		{
			return new UpdateLabelViewModel
			{
				Labels = new List<GitHubLabelViewModel>
						{
							new GitHubLabelViewModel()
						},
				Username = "username",
				Repository = "repository"
			};
		}

		private static void AssertLabelsMatch(IList<GitHubLabelViewModel> labels, IList<GitHubLabel> expectedLabels)
		{
			Assert.That(labels.Count, Is.EqualTo(expectedLabels.Count));

			for (var i = 0; i < labels.Count; i++)
			{
				CompareLabels(labels[i], expectedLabels[i]);
			}
		}

		private static void CompareLabels(GitHubLabelViewModel viewModel, GitHubLabel model)
		{
			Assert.That(viewModel.Color, Is.EqualTo(model.Color));
			Assert.That(viewModel.Name, Is.EqualTo(model.Name));
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

		private static LabelsController CreateController(IRepositoryManager repositoryManager = null, ILabelManager labelManager = null)
		{
			var mockRepositoryManager = new Mock<IRepositoryManager>();
			var mockLabelManager = new Mock<ILabelManager>();
			mockLabelManager.Setup(m => m.GetLabelsForRepository(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<GitHubLabel>());

			return new LabelsController(CreateMockRepositoryManagerFactory(repositoryManager ?? mockRepositoryManager.Object), CreateMockLabelManagerFactory(labelManager ?? mockLabelManager.Object));
		}

		private static IRepositoryManagerFactory CreateMockRepositoryManagerFactory(IRepositoryManager repositoryManager)
		{
			var factory = new Mock<IRepositoryManagerFactory>();
			factory.Setup(m => m.Create(It.IsAny<GitHubAccessToken>())).Returns(repositoryManager);

			return factory.Object;
		}

		private static ILabelManagerFactory CreateMockLabelManagerFactory(ILabelManager labelManager)
		{
			var factory = new Mock<ILabelManagerFactory>();
			factory.Setup(m => m.Create(It.IsAny<GitHubAccessToken>())).Returns(labelManager);

			return factory.Object;
		}
	}
}
