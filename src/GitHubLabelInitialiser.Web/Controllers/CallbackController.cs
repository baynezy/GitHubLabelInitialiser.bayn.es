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
			var token = _gitHubAuthenticator.Authenticate(model.Code, model.State).Result;

			if (!sessionState.Equals(model.State))
			{
				return new HttpStatusCodeResult(403, "Forbidden");
			}

			Session["GitHubAuthToken"] = token;

			return new RedirectResult("/labels");
		}
	}
}