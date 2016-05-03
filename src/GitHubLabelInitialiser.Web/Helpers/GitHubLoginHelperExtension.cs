using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public static class GitHubLoginHelperExtension
	{
		private const string BaseUrl = "https://github.com/login/oauth/authorize";

		public static MvcHtmlString LoginButton(this HtmlHelper htmlHelper, string clientId, string redirectUri = null, IList<GitHubScope> scope = null, string state = null)
		{
			var link = new TagBuilder("a");
			link.MergeAttribute("href", CreateLink(clientId, state ?? System.Guid.NewGuid().ToString(), redirectUri, scope));
			link.InnerHtml = "Create account with GitHub";
			var html = link.ToString();

			return MvcHtmlString.Create(html);
		}

		private static string CreateLink(string clientId, string state, string redirectUri = null, IEnumerable<GitHubScope> scope = null)
		{
			var optional = new StringBuilder();
			var stateQuery = "&state=" + state;

			if (redirectUri != null) optional.Append("&redirect_uri=" + redirectUri);
			if (scope != null) optional.Append("&scope=" + string.Join(",", scope));

			return string.Format(@"{0}?client_id={1}{2}{3}", BaseUrl, clientId, optional, stateQuery);
		}
	}
}