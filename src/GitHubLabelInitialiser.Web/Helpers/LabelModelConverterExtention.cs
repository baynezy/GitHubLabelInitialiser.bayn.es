using GitHubLabelInitialiser.Models;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public static class LabelModelConverterExtention
	{
		public static GitHubLabelViewModel ConvertToViewModel(this GitHubLabel label)
		{
			return new GitHubLabelViewModel
				{
					Color = label.Color,
					Name = label.Name
				};
		}

		public static GitHubLabel ConvertToModel(this GitHubLabelViewModel model)
		{
			return new GitHubLabel
			{
				Color = model.Color,
				Name = model.Name
			};
		}
	}
}