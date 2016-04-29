using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Helpers;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Controllers
{
	public class CallbackController : Controller
	{
		private readonly IGitHubAuthenticator _gitHubAuthenticator;

		public CallbackController(IGitHubAuthenticator gitHubAuthenticator)
		{
			_gitHubAuthenticator = gitHubAuthenticator;
		}

		public ActionResult GitHub(IUser user, GitHubAuthViewModel model)
		{
			var sessionState = user.GitHubAuthenticationState;
			var token = _gitHubAuthenticator.Authenticate(model.Code, model.State);

			return View(new GitHubAccessRequestViewModel
				{
					AccessToken = token
				});
		}
	}
}