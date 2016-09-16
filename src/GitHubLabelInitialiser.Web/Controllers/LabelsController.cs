using System.Threading.Tasks;
using System.Web.Mvc;
using GitHubLabelInitialiser.Web.Helpers;
using GitHubLabelInitialiser.Web.Models;

namespace GitHubLabelInitialiser.Web.Controllers
{
	public class LabelsController : Controller
	{
		private readonly IRepositoryManagerFactory _repositoryManagerFactory;
		private readonly ILabelManagerFactory _labelManagerFactory;

		public LabelsController(IRepositoryManagerFactory repositoryManagerFactory, ILabelManagerFactory labelManagerFactory)
		{
			_repositoryManagerFactory = repositoryManagerFactory;
			_labelManagerFactory = labelManagerFactory;
		}

		public async Task<ActionResult> Index(IUser user)
		{
			if (!IsAuthenticated(user))
			{
				return RedirectToAction("index", "home");
			}

			var repositoryManager = _repositoryManagerFactory.Create(user.GitHubAccessToken);

			var repositories = await repositoryManager.GetAllForAuthenticatedUser();

			return View(new ChooseRepositoryViewModel
				{
					Repositories = repositories
				});
		}

		public async Task<ViewResult> Update(string username, string repository, IUser user)
		{
			var labelManager = _labelManagerFactory.Create(user.GitHubAccessToken);

			var viewModel = new UpdateLabelViewModel
				{
					Labels = await labelManager.GetLabelsForRepository(username, repository)
				};
			return View(viewModel);
		}

		private static bool IsAuthenticated(IUser user)
		{
			return user.GitHubAccessToken != null;
		}
	}
}