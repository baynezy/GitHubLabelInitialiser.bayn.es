using System;
using System.Collections.Generic;

namespace GitHubLabelInitialiser.Web.Models
{
	public class HomeIndexViewModel
	{
		public string ClientId { get; set; }
		public Uri RedirectUri { get; set; }
		public IList<string> Scopes { get; set; }
	}
}