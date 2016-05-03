namespace GitHubLabelInitialiser.Web.Models
{
	public class User : IUser
	{
		public string GitHubAuthenticationState { get; set; }
		public GitHubAccessToken GitHubAccessToken { get; set; }
	}
}