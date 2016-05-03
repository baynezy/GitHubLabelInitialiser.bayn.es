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
	class CallbackControllerTest : TestControllerBase
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

			controller.GitHub(MockUser(state).Object, viewModel);

			authenticator.Verify(f => f.Authenticate(code, state), Times.Once());
		}

		[Test]
		public void GitHub_WhenCalledWithValidModel_ThenReturnRedirectResult()
		{
			const string code = "some-code";
			const string state = "some-state";

			var viewModel = new GitHubAuthViewModel { Code = code, State = state };

			var controller = CreateController();

			var result = controller.GitHub(MockUser(state).Object, viewModel) as RedirectResult;

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void GitHub_WhenCalledWithValidModel_ThenRedirectToAddLabels()
		{
			const string code = "some-code";
			const string state = "some-state";

			var viewModel = new GitHubAuthViewModel { Code = code, State = state };

			var controller = CreateController();

			var result = (RedirectResult) controller.GitHub(MockUser(state).Object, viewModel);

			Assert.That(result.Url, Is.EqualTo("/labels"));
			Assert.That(result.Permanent, Is.False);
		}

		[Test]
		public void GitHub_WhenCalledWithValidModel_ThenReadGitHubAuthStateFromSession()
		{
			const string code = "some-code";
			const string callbackState = "some-state";
			const string sessionState = "some-state";
			var controller = CreateController();
			var user = MockUser(sessionState);

			var viewModel = new GitHubAuthViewModel { Code = code, State = callbackState };

			controller.GitHub(user.Object, viewModel);

			user.Verify(f => f.GitHubAuthenticationState, Times.Once);
		}

		[Test]
		public void GitHub_WhenAuthenticationStateDoesntMatchSessionState_ThenReturn403StatusCode()
		{
			const string callbackState = "some-state";
			const string sessionState = "some-other-state";

			var controller = CreateController();
			var user = new Mock<IUser>();
			user.SetupGet(m => m.GitHubAuthenticationState).Returns(sessionState);

			var viewModel = new GitHubAuthViewModel {State = callbackState};

			var result = controller.GitHub(user.Object, viewModel) as HttpStatusCodeResult;

			Assert.That(result, Is.Not.Null);
// ReSharper disable PossibleNullReferenceException
			Assert.That(result.StatusCode, Is.EqualTo(403));
// ReSharper restore PossibleNullReferenceException
			Assert.That(result.StatusDescription, Is.EqualTo("Forbidden"));
		}

		[Test]
		public void GitHub_WhenSuccessfullyCalling_ThenPopululateTokenIntoSession()
		{
			const string state = "some-state";
			var expectedToken = new GitHubAccessToken
				{
					AccessToken = "some-token",
					Scope = new List<GitHubScope>
						{
							GitHubScope.PublicRepo
						},
					Type = GitHubTokenType.Bearer
				};
			var authenticator = new Mock<IGitHubAuthenticator>();
			authenticator.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedToken);

			var controller = CreateController(authenticator.Object);

			var user = MockUser(state);

			var viewModel = new GitHubAuthViewModel { State = state };

			controller.GitHub(user.Object, viewModel);

			Assert.That(Session["GitHubAuthToken"], Is.EqualTo(expectedToken));
		}

		private static Mock<IUser> MockUser(string sessionState)
		{
			var user = new Mock<IUser>();
			user.SetupGet(m => m.GitHubAuthenticationState).Returns(sessionState);
			return user;
		}

		private CallbackController CreateController(IGitHubAuthenticator gitHubAuthenticator = null)
		{
			var controller = new CallbackController(gitHubAuthenticator ?? new Mock<IGitHubAuthenticator>().Object);
			EnrichWithContext(controller);

			return controller;
		}
	}
}
