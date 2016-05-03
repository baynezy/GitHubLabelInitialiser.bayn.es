using GitHubLabelInitialiser.Web.Helpers;
using NUnit.Framework;

namespace GitHubLabelInitialiser.Web.Test.Helpers
{
	[TestFixture]
	class HttpHelperTest
	{
		[Test]
		public void HttpHelper_ImplementsIHttpHelper()
		{
			var helper = CreateHelper();

			Assert.That(helper, Is.InstanceOf<IHttpHelper>());
		}

		private static HttpHelper CreateHelper()
		{
			return new HttpHelper();
		}
	}
}
