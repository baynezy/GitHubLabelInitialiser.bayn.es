using System.Configuration;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public class Config : IConfig
	{
		public string GitHubClientId
		{
			get { return ConfigurationManager.AppSettings["GithubClientId"]; }
			
		}

		public string GitHubRedirectUrl
		{
			get { return ConfigurationManager.AppSettings["GitHubRedirectUrl"]; }
		}

		public string GitHubClientSecret {
			get
			{
				return System.Environment.GetEnvironmentVariable("GitHubClientSecret") ??
					   ConfigurationManager.AppSettings["GitHubClientSecret"];
			}
		}
	}
}