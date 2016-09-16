using System.Collections.Generic;
using GitHubLabelInitialiser.Models;

namespace GitHubLabelInitialiser.Web.Models
{
	public class UpdateLabelViewModel
	{
		public IList<GitHubLabel> Labels { get; set; }
	}
}