using GitHubLabelInitialiser.Web.Helpers;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace GitHubLabelInitialiser.Web.Test.Helpers
{
	[TestFixture]
	class GitHubStateGeneratorTest
	{
		[Test]
		public void GitHubStateGenerator_ImplementsIGitHubStateGenerator()
		{
			var generator = CreateGenerator();

			Assert.That(generator, Is.InstanceOf<IGitHubStateGenerator>());
		}

		private static GitHubStateGenerator CreateGenerator()
		{
			return new GitHubStateGenerator();
		}
	}
}
