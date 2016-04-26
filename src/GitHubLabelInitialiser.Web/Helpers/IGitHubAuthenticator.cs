using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public interface IGitHubAuthenticator
	{
		GitHubAccessToken Authenticate(string code, string state);
	}
}
