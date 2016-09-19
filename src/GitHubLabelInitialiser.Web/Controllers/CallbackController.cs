using System.Threading.Tasks;
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

		public async Task<ActionResult> GitHub(IUser user, GitHubAuthViewModel model)
		{
			var sessionState = user.GitHubAuthenticationState;
			var token = await _gitHubAuthenticator.Authenticate(model.Code, model.State);

			if (!sessionState.Equals(model.State))
			{
				return new HttpStatusCodeResult(403, "Forbidden");
			}

			Session["GitHubAuthToken"] = token;

			return RedirectToAction("index", "labels");
		}
	}
}