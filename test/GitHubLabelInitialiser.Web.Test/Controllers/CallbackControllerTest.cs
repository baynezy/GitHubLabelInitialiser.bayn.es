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
	class CallbackControllerTest
	{
		[Test]
		public void CallbackController_ImplementsController()
		{
			var controller = CreateController();
			
			Assert.That(controller, Is.InstanceOf<Controller>());
		}

		[Test]
		public void GitHub_WhenCalledWithValidModel_ThenCallIGitHubAuthenticatorAuthenticate()
		{
			var authenticator = new Mock<IGitHubAuthenticator>();
			var controller = CreateController(authenticator.Object);
			const string code = "some-code";
			const string state = "some-state";

			var viewModel = new GitHubAuthViewModel { Code = code, State = state };

			controller.GitHub(new User(), viewModel);

			authenticator.Verify(f => f.Authenticate(code, state), Times.Once());
		}

		[Test]
		public void GitHub_WhenCalledWithValidModel_ThenReturnViewResult()
		{
			var controller = CreateController();
			const string code = "some-code";
			const string state = "some-state";

			var viewModel = new GitHubAuthViewModel { Code = code, State = state };

			var result = controller.GitHub(new User(), viewModel) as ViewResult;

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void GitHub_WhenCalledWithValidModel_ThenReturnViewModel()
		{
			var controller = CreateController();
			const string code = "some-code";
			const string state = "some-state";

			var viewModel = new GitHubAuthViewModel { Code = code, State = state };

			var result = (ViewResult)controller.GitHub(new User(), viewModel);

			Assert.That(result.Model, Is.InstanceOf<GitHubAccessRequestViewModel>());
		}

		[Test]
		public void GitHub_WhenCalledWithValidModel_ThenPopulateViewModelWithToken()
		{
			var authenticator = new Mock<IGitHubAuthenticator>();
			var token = new GitHubAccessToken
				{
					AccessToken = "e72e16c7e42f292c6912e7710c838347ae178b4a",
					Scope = new List<GitHubScope> {GitHubScope.PublicRepo},
					Type = GitHubTokenType.Bearer
				};
			authenticator.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(token);
			var controller = CreateController(authenticator.Object);
			const string code = "some-code";
			const string state = "some-state";

			var viewModel = new GitHubAuthViewModel { Code = code, State = state };

			var result = (ViewResult)controller.GitHub(new User(), viewModel);
			var model = (GitHubAccessRequestViewModel)result.Model;

			Assert.That(model.AccessToken, Is.EqualTo(token));
		}

		[Test]
		public void GitHub_WhenCalledWithValidModel_ThenReadGitHubAuthStateFromSession()
		{
			const string code = "some-code";
			const string callbackState = "some-state";
			const string sessionState = "some-state";
			var controller = CreateController();
			var user = new Mock<IUser>();
			user.SetupGet(m => m.GitHubAuthenticationState).Returns(sessionState);

			var viewModel = new GitHubAuthViewModel { Code = code, State = callbackState };

			controller.GitHub(user.Object, viewModel);

			user.Verify(f => f.GitHubAuthenticationState, Times.Once);
		}

		private static CallbackController CreateController(IGitHubAuthenticator gitHubAuthenticator = null)
		{
			return new CallbackController(gitHubAuthenticator ?? new Mock<IGitHubAuthenticator>().Object);
		}
	}
}
