using System;
using System.Collections.Generic;
using GitHubLabelInitialiser.Web.Helpers;
using Moq;
using NUnit.Framework;

namespace GitHubLabelInitialiser.Web.Test.Helpers
{
	[TestFixture]
	class GitHubAuthenticatorTest
	{
		[Test]
		public void GitHubAuthenticator_ImplementsIGitHubAuthenticator()
		{
			var authenticator = CreateAuthenticator();

			Assert.That(authenticator, Is.InstanceOf<IGitHubAuthenticator>());
		}

		[Test]
		public void Authenticate_WhenCalled_ThenCallIHttpHelperPost()
		{
			const string code = "some-code";
			const string state = "some-state";
			var uri = new Uri("https://github.com/login/oauth/access_token");
			var parameters = new Dictionary<string, string>
				{
					{ "client_id", "testClientId" },
					{ "client_secret", "secret" },
					{ "code", code },
					{ "state", state }
				};
			const string acceptsHeader = "application/json";
			IDictionary<string, string> actualParams = new Dictionary<string, string>();
			var httpHelper = MockHttpHelper((u, p, s) => { actualParams = p; });

			var authenticator = CreateAuthenticator(httpHelper.Object);

			authenticator.Authenticate(code, state);

			httpHelper.Verify(f => f.Post(uri, It.IsAny<IDictionary<string,string>>(), acceptsHeader), Times.Once);
			Assert.That(actualParams, Is.EquivalentTo(parameters));
		}

		[Test]
		public async void Authenticate_WhenCalled_CreateTokenFromReturnedHttpResponse()
		{
			const string code = "some-code";
			const string state = "some-state";

			var httpHelper = MockHttpHelper((u, p, s) => { });

			var authenticator = CreateAuthenticator(httpHelper.Object);

			var token = await authenticator.Authenticate(code, state);

			Assert.That(token.AccessToken, Is.EqualTo("e72e16c7e42f292c6912e7710c838347ae178b4a"));
			Assert.That(token.Scope.Count, Is.EqualTo(1));
			Assert.That(token.Scope[0].Value, Is.EqualTo(GitHubScope.PublicRepo.Value));
			Assert.That(token.Type, Is.EqualTo(GitHubTokenType.Bearer));
			
		}

		private static Mock<IHttpHelper> MockHttpHelper(Action<Uri,IDictionary<string,string>,string> callback)
		{
			var httpHelper = new Mock<IHttpHelper>();
			httpHelper.Setup(m => m.Post(It.IsAny<Uri>(), It.IsAny<Dictionary<string,string>>(), It.IsAny<string>()))
				.ReturnsAsync(
				          @"{""access_token"":""e72e16c7e42f292c6912e7710c838347ae178b4a"", ""scope"":""public_repo"", ""token_type"":""bearer""}")
				.Callback(callback);
			return httpHelper;
		}

		private static IGitHubAuthenticator CreateAuthenticator(IHttpHelper httpHelper = null)
		{
			var config = new Config();
			return new GitHubAuthenticator(config, httpHelper ?? MockHttpHelper((u, p, s) => { }).Object);
		}
	}
}
