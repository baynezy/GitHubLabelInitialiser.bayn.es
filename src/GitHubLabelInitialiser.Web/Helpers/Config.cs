using System.Configuration;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public class Config : IConfig
	{
		public string GitHubClientId()
		{
			return ConfigurationManager.AppSettings["GithubClientId"];
		}

		public string GitHubRedirectUrl()
		{
			return ConfigurationManager.AppSettings["GitHubRedirectUrl"];
		}
	}
}