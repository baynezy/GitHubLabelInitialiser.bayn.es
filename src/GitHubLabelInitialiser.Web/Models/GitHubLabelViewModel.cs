using System.ComponentModel.DataAnnotations;

namespace GitHubLabelInitialiser.Web.Models
{
	public class GitHubLabelViewModel
	{
		[Required]
		[MinLength(1)]
		public string Color { get; set; }

		[Required]
		[MinLength(6)]
		[MaxLength(6)]
		public string Name { get; set; }
	}
}