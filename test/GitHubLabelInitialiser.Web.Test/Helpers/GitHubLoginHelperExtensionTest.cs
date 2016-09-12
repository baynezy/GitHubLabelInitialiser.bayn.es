using System.Collections.Generic;
using System.Text.RegularExpressions;
using GitHubLabelInitialiser.Web.Helpers;
using MvcTestHelper;
using NUnit.Framework;

namespace GitHubLabelInitialiser.Web.Test.Helpers
{
	[TestFixture]
	class GitHubLoginHelperExtensionTest
	{
		[Test]
		public void LoginButton_WhenPassingInClientIdOnly_ThenReturnButtonHtml()
		{
			const string clientId = "wereteWHDSAWETW";
			var helper = MvcHelper.GetHtmlHelper();
			var html = helper.LoginButton(clientId);
			var expectedWithoutState = string.Format(
					@"<a href=""https://github.com/login/oauth/authorize?client_id={0}"">Create account with GitHub</a>", clientId);
			var stateRemoved = RemoveState(html.ToHtmlString());

			Assert.That(stateRemoved, Is.EqualTo(expectedWithoutState));
		}

		[Test]
		public void LoginButton_WhenPassingInClientIdAndRedirectUrl_ThenReturnButtonHtml()
		{
			const string clientId = "wereteWHDSAWETW";
			const string redirectUri = "http://localhost/callback/github/";
			var helper = MvcHelper.GetHtmlHelper();
			var html = helper.LoginButton(clientId, redirectUri);
			var expectedWithoutState =
				string.Format(
					@"<a href=""https://github.com/login/oauth/authorize?client_id={0}&amp;redirect_uri={1}"">Create account with GitHub</a>",
					clientId, redirectUri);
			var stateRemoved = RemoveState(html.ToHtmlString());

			Assert.That(stateRemoved, Is.EqualTo(expectedWithoutState));
		}

		[Test]
		public void LoginButton_WhenPassingInClientIdAndRedirectUrlAndScope_ThenReturnButtonHtml()
		{
			const string clientId = "wereteWHDSAWETW";
			const string redirectUri = "http://localhost/callback/github/";
			var scope = new List<GitHubScope> { GitHubScope.PublicRepo, GitHubScope.User };
			var helper = MvcHelper.GetHtmlHelper();
			var html = helper.LoginButton(clientId, redirectUri, scope);
			var expectedWithoutState =
				string.Format(
					@"<a href=""https://github.com/login/oauth/authorize?client_id={0}&amp;redirect_uri={1}&amp;scope={2}"">Create account with GitHub</a>",
					clientId, redirectUri, "public_repo,user");
			var stateRemoved = RemoveState(html.ToHtmlString());

			Assert.That(stateRemoved, Is.EqualTo(expectedWithoutState));
		}

		[Test]
		public void LoginButton_WhenPassingInClientIdAndState_ThenReturnButtonHtml()
		{
			const string clientId = "wereteWHDSAWETW";
			var state = RandomString();
			var helper = MvcHelper.GetHtmlHelper();
			var html = helper.LoginButton(clientId, state: state);
			var expected = string.Format(
					@"<a href=""https://github.com/login/oauth/authorize?client_id={0}&amp;state={1}"">Create account with GitHub</a>", clientId, state);

			Assert.That(html.ToHtmlString(), Is.EqualTo(expected));
		}

		[Test]
		public void LoginButton_WhenPassingInClientIdAndRedirectUrlAndState_ThenReturnButtonHtml()
		{
			const string clientId = "wereteWHDSAWETW";
			const string redirectUri = "http://localhost/callback/github/";
			var state = RandomString();
			var helper = MvcHelper.GetHtmlHelper();
			var html = helper.LoginButton(clientId, redirectUri, state: state);
			var expected =
				string.Format(
					@"<a href=""https://github.com/login/oauth/authorize?client_id={0}&amp;redirect_uri={1}&amp;state={2}"">Create account with GitHub</a>",
					clientId, redirectUri, state);


			Assert.That(html.ToHtmlString(), Is.EqualTo(expected));
		}

		[Test]
		public void LoginButton_WhenPassingInClientIdAndRedirectUrlAndScopeAndState_ThenReturnButtonHtml()
		{
			const string clientId = "wereteWHDSAWETW";
			const string redirectUri = "http://localhost/callback/github/";
			var state = RandomString();
			var scope = new List<GitHubScope> { GitHubScope.PublicRepo, GitHubScope.User };
			var helper = MvcHelper.GetHtmlHelper();
			var html = helper.LoginButton(clientId, redirectUri, scope, state);
			var expected =
				string.Format(
					@"<a href=""https://github.com/login/oauth/authorize?client_id={0}&amp;redirect_uri={1}&amp;scope={2}&amp;state={3}"">Create account with GitHub</a>",
					clientId, redirectUri, "public_repo,user", state);

			Assert.That(html.ToHtmlString(), Is.EqualTo(expected));
		}

		private static string RandomString()
		{
			return System.Guid.NewGuid().ToString();
		}

		private static string RemoveState(string html)
		{
			return Regex.Replace(html, @"&amp;state=[^""]+", "");
		}

		[Test]
		public void LoginButton_WhenCalling_ThenShouldHaveDifferentStateOnSubsequentCalls()
		{
			const string clientId = "wereteWHDSAWETW";
			const string redirectUri = "http://localhost/callback/github/";
			var scope = new List<GitHubScope> { GitHubScope.PublicRepo, GitHubScope.User };
			var helper = MvcHelper.GetHtmlHelper();
			var html = helper.LoginButton(clientId, redirectUri, scope).ToHtmlString();
			var html2 = helper.LoginButton(clientId, redirectUri, scope).ToHtmlString();

			Assert.That(html, Is.Not.EqualTo(html2));
		}

		[Test]
		public void LoginButton_WhenCalling_ThenShouldHaveGeneratedState()
		{
			const string clientId = "wereteWHDSAWETW";
			const string redirectUri = "http://localhost/callback/github/";
			var scope = new List<GitHubScope> { GitHubScope.PublicRepo, GitHubScope.User };
			var helper = MvcHelper.GetHtmlHelper();
			var html = helper.LoginButton(clientId, redirectUri, scope).ToHtmlString();
			var match = new Regex("amp;state=.+").Match(html).Success;

			Assert.That(match, Is.True);
		}
	}
}
