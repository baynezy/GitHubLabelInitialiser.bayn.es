namespace GitHubLabelInitialiser.Web.Helpers
{
	public interface IConfig
	{
		string GitHubClientId { get; }
		string GitHubRedirectUrl { get; }
		string GitHubClientSecret { get;  }
		string AppName { get; }
	}
}
