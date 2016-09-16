using GitHubLabelInitialiser.Web.Helpers;
using NUnit.Framework;

namespace GitHubLabelInitialiser.Web.Test.Helpers
{
	[TestFixture]
	class LabelManagerFactoryTest
	{
		[Test]
		public void LabelManagerFactory_ImplementsILabelManagerFactory()
		{
			var factory = CreateFactory();

			Assert.That(factory, Is.InstanceOf<ILabelManagerFactory>());
		}

		private static ILabelManagerFactory CreateFactory()
		{
			return new LabelManagerFactory();
		}
	}
}
