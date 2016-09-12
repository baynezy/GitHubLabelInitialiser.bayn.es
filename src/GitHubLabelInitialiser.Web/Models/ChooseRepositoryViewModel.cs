using System.Collections.Generic;
using GitHubLabelInitialiser.Models;

namespace GitHubLabelInitialiser.Web.Models
{
	public class ChooseRepositoryViewModel
	{
		public IList<GitHubRepository> Repositories { get; set; }
	}
}