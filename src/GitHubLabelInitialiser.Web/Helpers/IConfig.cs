namespace GitHubLabelInitialiser.Web.Helpers
{
	public interface IConfig
	{
		string GitHubClientId();
		string GitHubRedirectUrl();
	}
}
