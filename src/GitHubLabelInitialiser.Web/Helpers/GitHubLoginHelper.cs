namespace GitHubLabelInitialiser.Web.Helpers
{
	public class GitHubLoginHelper
	{
		private readonly string _baseUrl;

		public GitHubLoginHelper(string baseUrl)
		{
			_baseUrl = baseUrl;
		}

		public string LoginButton(string clientId)
		{
			return string.Format(@"<a href=""{0}?client_id={1}"">Create account with GitHub</a>", _baseUrl, clientId);
		}
	}
}