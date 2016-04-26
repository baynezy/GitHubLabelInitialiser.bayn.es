namespace GitHubLabelInitialiser.Web.Helpers
{
	public class GitHubStateGenerator : IGitHubStateGenerator
	{
		public string GenerateState()
		{
			return System.Guid.NewGuid().ToString();
		}
	}
}