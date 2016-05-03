using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitHubLabelInitialiser.Web.Models;
using Newtonsoft.Json.Linq;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public class GitHubAuthenticator : IGitHubAuthenticator
	{
		private readonly IConfig _config;
		private readonly IHttpHelper _httpHelper;
		private readonly Uri _authUrl = new Uri("https://github.com/login/oauth/access_token");

		public GitHubAuthenticator(IConfig config, IHttpHelper httpHelper)
		{
			_config = config;
			_httpHelper = httpHelper;
		}

		public async Task<GitHubAccessToken> Authenticate(string code, string state)
		{
			var response = await _httpHelper.Post(_authUrl, new Dictionary<string, string>
				{
					{"client_id", _config.GitHubClientId},
					{"client_secret", _config.GitHubClientSecret},
					{"code", code},
					{"state", state}
				}, "application/json");
			return ParseToken(response);
		}

		private static GitHubAccessToken ParseToken(string response)
		{
			var json = JObject.Parse(response);
			return new GitHubAccessToken
				{
					AccessToken = (string) json.GetValue("access_token"),
					Type = PopulateType((string) json.GetValue("token_type")),
					Scope = PopulateScope((string) json.GetValue("scope"))
				};
		}

		private static IList<GitHubScope> PopulateScope(string scope)
		{
			var scopes = new List<GitHubScope>();
			IEnumerable<string> split = scope.Split(',');

			split.ToList().ForEach(s =>
				{
					var parsed = GitHubScope.TryParse(s);

					if (parsed != null)
					{
						scopes.Add(parsed);
					}
				});

			return scopes;
		}

		private static GitHubTokenType PopulateType(string type)
		{
			GitHubTokenType tType;
			Enum.TryParse(type, true, out tType);

			return tType;
		}
	}
}