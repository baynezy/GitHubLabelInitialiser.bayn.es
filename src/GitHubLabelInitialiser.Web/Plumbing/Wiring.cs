using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GitHubLabelInitialiser.Web.Helpers;
using GitHubLabelInitialiser.Web.Models;

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
					.LifeStyle.Transient,
				Component.For<IRepositoryManagerFactory>()
					.ImplementedBy<RepositoryManagerFactory>()
					.LifeStyle.Transient,
				Component.For<ILabelManagerFactory>()
					.ImplementedBy<LabelManagerFactory>()
					.LifeStyle.Transient,
				Component.For<IMeta>()
					.ImplementedBy<Meta>()
					.LifeStyle.Transient
				);
		}
	}
}