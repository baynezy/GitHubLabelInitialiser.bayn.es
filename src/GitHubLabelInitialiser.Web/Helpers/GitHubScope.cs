﻿namespace GitHubLabelInitialiser.Web.Helpers
{
	public class GitHubScope
	{
		private GitHubScope(string value)
		{
			Value = value;
		}

		public string Value { get; private set; }

		public static GitHubScope PublicRepo { get { return new GitHubScope("public_repo"); } }
	}
}