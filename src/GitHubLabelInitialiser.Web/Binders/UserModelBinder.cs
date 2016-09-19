using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Binders
{
	public class UserModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var user = new User();
			if (controllerContext.HttpContext.Session != null)
			{
				user.GitHubAuthenticationState = controllerContext.HttpContext.Session["GitHubAuthenticationState"] as string;
				user.GitHubAccessToken = controllerContext.HttpContext.Session["GitHubAuthToken"] as GitHubAccessToken;
			}

			return user;
		}
	}
}