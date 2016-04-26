namespace GitHubLabelInitialiser.Web.Helpers
{
	public interface IGitHubAuthenticator
	{
		void Authenticate(string someCode, string someState);
	}
}
