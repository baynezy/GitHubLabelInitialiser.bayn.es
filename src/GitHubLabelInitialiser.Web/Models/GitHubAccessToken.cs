using System.Collections.Generic;
using GitHubLabelInitialiser.Web.Helpers;

namespace GitHubLabelInitialiser.Web.Models
{
	public class GitHubAccessToken
	{
		public string AccessToken { get; set; }

		public IList<GitHubScope> Scope { get; set; }

		public GitHubTokenType Type { get; set; }
	}
}