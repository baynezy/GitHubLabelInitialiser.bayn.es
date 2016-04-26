using System.Collections.Generic;
using GitHubLabelInitialiser.Web.Helpers;

namespace GitHubLabelInitialiser.Web.Models
{
	public class GitHubAccessToken
	{
		public string AccessToken { get; set; }

		public List<string> Scope { get; set; }

		public GitHubTokenType Type { get; set; }
	}
}