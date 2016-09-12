using System.Collections.Generic;
using GitHubLabelInitialiser.Web.Helpers;
using GitHubLabelInitialiser.Web.Models;
using Moq;
using NUnit.Framework;

namespace GitHubLabelInitialiser.Web.Test.Helpers
{
	[TestFixture]
	class RepositoryManagerFactoryTest
	{
		[Test]
		public void RepositoryManagerFactory_ImplementsIRepositoryManagerFactory()
		{
			var factory = CreateFactory();

			Assert.That(factory, Is.InstanceOf<IRepositoryManagerFactory>());
		}

		private static IRepositoryManagerFactory CreateFactory()
		{
			var config = new Mock<IConfig>();
			config.SetupGet(p => p.AppName).Returns("some-app");
			return new RepositoryManagerFactory(config.Object);
		}
	}
}
