using System.Reflection;

namespace GitHubLabelInitialiser.Web.Models
{
	public class Meta : IMeta
	{
		public string AppVersion()
		{
			var assembly = Assembly.GetCallingAssembly();
			var version = assembly.GetName().Version;
			return version.ToString();
		}
	}
}