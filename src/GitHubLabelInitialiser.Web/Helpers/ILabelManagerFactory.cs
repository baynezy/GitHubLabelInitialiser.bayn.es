using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public interface ILabelManagerFactory
	{
		ILabelManager Create(GitHubAccessToken token);
	}
}
