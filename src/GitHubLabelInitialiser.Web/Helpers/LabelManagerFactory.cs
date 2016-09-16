using GitHubLabelInitialiser.Models;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public class LabelManagerFactory : ILabelManagerFactory
	{
		private IConfig _config;

		public LabelManagerFactory(IConfig config)
		{
			_config = config;
		}

		public ILabelManager Create(GitHubAccessToken token)
		{
			var gitHubAccess = new GitHubToken { Token = token.AccessToken };
			var gitHubApi = new GitHubApi(gitHubAccess, _config.AppName);

			return new LabelManager(gitHubApi);
		}
	}
}