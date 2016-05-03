using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public interface IHttpHelper
	{
		Task<string> Post(Uri url, IDictionary<string,string> parameters, string acceptsHeader);
	}
}
