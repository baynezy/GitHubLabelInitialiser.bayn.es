using System.Linq;
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
			var labels = await labelManager.GetLabelsForRepository(username, repository);

			var viewModel = new UpdateLabelViewModel
				{
					Labels = labels.Select(l => l.ConvertToViewModel()).ToList()
				};
			return View(viewModel);
		}

		private static bool IsAuthenticated(IUser user)
		{
			return user.GitHubAccessToken != null;
		}

		public async Task<ViewResult> Initialise(string username, string repository, UpdateLabelViewModel viewModel, IUser user)
		{
			var labelManager = _labelManagerFactory.Create(user.GitHubAccessToken);
			var labels = viewModel.Labels.Select(l => l.ConvertToModel()).ToList();
			await labelManager.IntialiseLabels(username, repository, labels);

			return View();
		}
	}
}