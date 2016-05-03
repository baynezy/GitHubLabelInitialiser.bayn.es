using System;
using System.Collections.Generic;
using GitHubLabelInitialiser.Web.Helpers;

namespace GitHubLabelInitialiser.Web.Models
{
	public class HomeIndexViewModel
	{
		public string ClientId { get; set; }
		public Uri RedirectUri { get; set; }
		public IList<GitHubScope> Scopes { get; set; }
		public string State { get; set; }
	}
}