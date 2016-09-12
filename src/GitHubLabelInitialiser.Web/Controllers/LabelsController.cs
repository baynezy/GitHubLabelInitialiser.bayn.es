using System.Threading.Tasks;
using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Helpers;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Controllers
{
	public class LabelsController : Controller
	{
		private readonly IRepositoryManagerFactory _repositoryManagerFactory;

		public LabelsController(IRepositoryManagerFactory repositoryManagerFactory)
		{
			_repositoryManagerFactory = repositoryManagerFactory;
		}

		public async Task<ActionResult> Index(IUser user)
		{
			if (!IsAuthenticated(user))
			{
				return RedirectToAction("index", "home");
			}

			var manager = _repositoryManagerFactory.Create(user.GitHubAccessToken);

			var repositories = await manager.GetAllForAuthenticatedUser();

			return View(new ChooseRepositoryViewModel
				{
					Repositories = repositories
				});
		}

		private static bool IsAuthenticated(IUser user)
		{
			return user.GitHubAccessToken != null;
		}
	}
}