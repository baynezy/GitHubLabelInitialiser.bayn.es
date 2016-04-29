using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using GitHubLabelInitialiser.Web.Binders;
using GitHubLabelInitialiser.Web.Models;
using GitHubLabelInitialiser.Web.Plumbing;

namespace GitHubLabelInitialiser.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication
	{
		private IWindsorContainer _container;
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			ModelBinders.Binders.Add(typeof(User), new UserModelBinder());

			_container = new WindsorContainer().Install(FromAssembly.This());
			ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_container.Kernel));
		}
	}
}