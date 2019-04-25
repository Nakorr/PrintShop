using System;
using Unity;
using Unity.Lifetime;
using System.Data.Entity;
using PrintShopServiceDAL.Interfaces;
using PrintShopServiceImplementDataBase;
using PrintShopServiceImplementDataBase.Implementations;

namespace PrintShopRestApi
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> �ontainer =
        new Lazy<IUnityContainer>(() =>
        {
            var �ontainer = new UnityContainer();
            RegisterTypes(�ontainer);
            return �ontainer;
        });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => �ontainer.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer �ontainer)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            Container.RegisterType<DbContext, PrintShopDbContext>(new
           HierarchicalLifetimeManager());
            Container.RegisterType<ICustomerService, CustomerServiceDB>(new
           HierarchicalLifetimeManager());
            Container.RegisterType<IIngredientService, IngredientServiceDB>(new
           HierarchicalLifetimeManager());
            Container.RegisterType<IPrintService, PrintServiceDB>(new
           HierarchicalLifetimeManager());
            Container.RegisterType<IMainService, MainServiceDB>(new
           HierarchicalLifetimeManager());
            Container.RegisterType<IStockService, StockServiceDB>(new
           HierarchicalLifetimeManager());
            Container.RegisterType<IReportService, ReportServiceDB>(new
           HierarchicalLifetimeManager());
        }
    }
}