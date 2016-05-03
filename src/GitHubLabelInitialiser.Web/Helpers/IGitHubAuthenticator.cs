using System.Threading.Tasks;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public interface IGitHubAuthenticator
	{
		Task<GitHubAccessToken> Authenticate(string code, string state);
	}
}
