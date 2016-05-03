using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GitHubLabelInitialiser.Web.Helpers
{
	public class HttpHelper : IHttpHelper
	{
		public async Task<string> Post(Uri url, IDictionary<string, string> parameters, string acceptsHeader)
		{
			HttpResponseMessage response = null;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptsHeader));

				response = client.PostAsync(url.ToString(), new FormUrlEncodedContent(parameters)).Result;
			}

			return await response.Content.ReadAsStringAsync();
		}
	}
}