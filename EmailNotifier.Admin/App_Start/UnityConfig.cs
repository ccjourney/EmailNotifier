using System;
using System.Configuration;
using EmailNotifier.Data;
using EmailNotifier.Publisher;
using Unity;
using Unity.Injection;

namespace EmailNotifier.Admin
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            var endpoint = ConfigurationManager.AppSettings["endpoint"];
            var authKey = ConfigurationManager.AppSettings["authKey"];
            var database = ConfigurationManager.AppSettings["database"];
            var collection = ConfigurationManager.AppSettings["collection"];
            var serviceBusConnectionString = ConfigurationManager.AppSettings["serviceBusConnectionString"];
            var topic = ConfigurationManager.AppSettings["topic"];
            container.RegisterType<INewUserRepository, NewUserRepository>(new InjectionConstructor(database, collection, endpoint, authKey));
            container.RegisterType<IEmailPublisher, EmailPublisher>(new InjectionConstructor(serviceBusConnectionString, topic));
        }
    }
}