using System.Collections.Generic;

namespace GitHubLabelInitialiser.Web.Models
{
	public class UpdateLabelViewModel
	{
		public IList<GitHubLabelViewModel> Labels { get; set; }

		public string Username { get; set; }

		public string Repository { get; set; }
	}
}