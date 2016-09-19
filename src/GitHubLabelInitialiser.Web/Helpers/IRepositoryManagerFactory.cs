using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public interface IRepositoryManagerFactory
	{
		IRepositoryManager Create(GitHubAccessToken token);
	}
}
