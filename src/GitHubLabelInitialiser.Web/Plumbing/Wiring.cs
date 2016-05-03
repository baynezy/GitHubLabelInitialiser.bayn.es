using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GitHubLabelInitialiser.Web.Helpers;

namespace GitHubLabelInitialiser.Web.Plumbing
{
	public class Wiring : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<IConfig>()
					.ImplementedBy<Config>()
					.LifeStyle.Transient,
				Component.For<IGitHubAuthenticator>()
					.ImplementedBy<GitHubAuthenticator>()
					.LifeStyle.Transient,
				Component.For<IGitHubStateGenerator>()
					.ImplementedBy<GitHubStateGenerator>()
					.LifeStyle.Transient,
				Component.For<IHttpHelper>()
					.ImplementedBy<HttpHelper>()
					.LifeStyle.Transient
				);
		}
	}
}