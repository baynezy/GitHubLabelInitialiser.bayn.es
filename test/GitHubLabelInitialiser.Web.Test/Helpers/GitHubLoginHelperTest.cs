using GitHubLabelInitialiser.Web.Helpers;
using NUnit.Framework;

namespace GitHubLabelInitialiser.Web.Test.Helpers
{
	[TestFixture]
	class GitHubLoginHelperTest
	{
		private static string baseURL = "http://localhost/";
		[Test]
		public void LoginButton_WhenPassingInClientIdOnly_ThenReturnButtonHtml()
		{
			const string clientId = "wereteWHDSAWETW";
			var helper = CreateHelper();
			var button = helper.LoginButton(clientId);

			AssertLoginButton(button, clientId);
		}

		private static GitHubLoginHelper CreateHelper()
		{
			return new GitHubLoginHelper(baseURL);
		}

		private static void AssertLoginButton(string button, string clientId)
		{
			var constructedButton = @"<a href=""" + baseURL + "?client_id=" + clientId + @""">Create account with GitHub</a>";

			Assert.That(button, Is.EqualTo(constructedButton));
		}
	}
}
