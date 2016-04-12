using System.Collections.Generic;
using System.Text.RegularExpressions;
using GitHubLabelInitialiser.Web.Helpers;
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
			var scope = new List<string> { "user", "user:email" };
			var helper = MvcHelper.GetHtmlHelper();
			var html = helper.LoginButton(clientId, redirectUri, scope);
			var expectedWithoutState =
				string.Format(
					@"<a href=""https://github.com/login/oauth/authorize?client_id={0}&amp;redirect_uri={1}&amp;scope={2}"">Create account with GitHub</a>",
					clientId, redirectUri, string.Join(",", scope));
			var stateRemoved = RemoveState(html.ToHtmlString());

			Assert.That(stateRemoved, Is.EqualTo(expectedWithoutState));
		}

		private static string RemoveState(string html)
		{
			return Regex.Replace(html, @"&amp;state=[^""]+", "");
		}

		[Test]
		public void LoginButton_WhenCalling_ThenShouldHaveDifferentStateOnSubsequesntCalls()
		{
			const string clientId = "wereteWHDSAWETW";
			const string redirectUri = "http://localhost/callback/github/";
			var scope = new List<string> { "user", "user:email" };
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
			var scope = new List<string> { "user", "user:email" };
			var helper = MvcHelper.GetHtmlHelper();
			var html = helper.LoginButton(clientId, redirectUri, scope).ToHtmlString();
			var match = new Regex("amp;state=.+").Match(html).Success;

			Assert.That(match, Is.True);
		}
	}
}
