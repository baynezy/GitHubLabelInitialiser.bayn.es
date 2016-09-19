using GitHubLabelInitialiser.Web.Helpers;
using Moq;
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
			var config = new Mock<IConfig>();
			config.SetupGet(p => p.AppName).Returns("some-app");
			return new LabelManagerFactory(config.Object);
		}
	}
}
