using System.Reflection;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public class GitHubScope
	{
		private GitHubScope(string value)
		{
			Value = value;
		}

		public string Value { get; private set; }

		public static GitHubScope PublicRepo { get { return new GitHubScope("public_repo"); } }
		public static GitHubScope User { get { return new GitHubScope("user"); } }

		public override string ToString()
		{
			return Value;
		}

		public static GitHubScope TryParse(string scope)
		{
			GitHubScope parsed = null;

			var type = typeof (GitHubScope);
			var properties = type.GetProperties(BindingFlags.Static | BindingFlags.Public);

			foreach (var propertyInfo in properties)
			{
				var property = (GitHubScope)propertyInfo.GetGetMethod().Invoke(null, new object[0]);
				if (property.Value.Equals(scope))
				{
					parsed = property;
					break;
				}
			}

			return parsed;
		}
	}
}