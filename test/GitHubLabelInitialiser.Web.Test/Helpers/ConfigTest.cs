using GitHubLabelInitialiser.Web.Helpers;
using NUnit.Framework;

namespace GitHubLabelInitialiser.Web.Test.Helpers
{
	[TestFixture]
	class ConfigTest
	{
		[Test]
		public void Config_ImplementsIConfig()
		{
			var config = CreateConfig();

			Assert.That(config, Is.InstanceOf<IConfig>());
		}

		[Test]
		public void GitHubClientId_WhenCalled_ThenReturnValueFromApplicationConfig()
		{
			var config = CreateConfig();

			Assert.That(config.GitHubClientId(), Is.EqualTo("testClientId"));
		}

		[Test]
		public void GitHubRedirectUrl_WhenCalled_ThenReturnValueFromApplicationconfig()
		{
			var config = CreateConfig();

			Assert.That(config.GitHubRedirectUrl(), Is.EqualTo("redirctUrl"));
		}

		private static IConfig CreateConfig()
		{
			return new Config();
		}
	}
}
