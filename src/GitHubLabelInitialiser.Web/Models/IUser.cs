namespace GitHubLabelInitialiser.Web.Models
{
	public interface IUser
	{
		string GitHubAuthenticationState { get; }
		GitHubAccessToken GitHubAccessToken { get; set; }
	}
}
