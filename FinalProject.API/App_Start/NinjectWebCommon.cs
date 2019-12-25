[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(FinalProject.API.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(FinalProject.API.App_Start.NinjectWebCommon), "Stop")]

namespace FinalProject.API.App_Start
{
    using System;
    using System.Web;
    using FinalProject.BLL.IServices;
    using FinalProject.BLL.Services;
    using FinalProject.DAL;
    using FinalProject.DAL.DBConnection;
    using FinalProject.DAL.IRepositories;
    using FinalProject.DAL.Repositories;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ISqlProgress>().To<SqlProgress>();
            kernel.Bind<IFolderService>().To<FolderService>();
            kernel.Bind<INodeService>().To<NodeService>();
            kernel.Bind<IFolderRepository>().To<FolderRepository>();

        }        
    }
}
