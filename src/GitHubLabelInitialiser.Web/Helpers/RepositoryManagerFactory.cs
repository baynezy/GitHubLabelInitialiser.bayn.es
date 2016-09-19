using GitHubLabelInitialiser.Models;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public class RepositoryManagerFactory : IRepositoryManagerFactory
	{
		private readonly IConfig _config;

		public RepositoryManagerFactory(IConfig config)
		{
			_config = config;
		}

		public IRepositoryManager Create(GitHubAccessToken token)
		{
			var gitHubAccess = new GitHubToken {Token = token.AccessToken};
			var gitHubApi = new GitHubApi(gitHubAccess, _config.AppName);
			return new RepositoryManager(gitHubApi);
		}
	}
}