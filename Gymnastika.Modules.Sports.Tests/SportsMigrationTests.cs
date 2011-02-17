using Gymnastika.Modules.Sports.Views;
using System;
using Gymnastika.Modules.Sports.Services;
using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Gymnastika.Modules.Sports.Models;
using System.ComponentModel;
using NUnit.Framework;
using Moq;
using Gymnastika.Modules.Sports.ViewModels;
using System.IO;
using Microsoft.Practices.Unity;
using Gymnastika.Data.Providers;
using Gymnastika.Data.SessionManagement;
using Gymnastika.Data;
using Gymnastika.Data.Migration;
using Gymnastika.Data.Configuration;
using Microsoft.Practices.ServiceLocation;


namespace Gymnastika.Modules.Sports.Test
{

    [TestFixture]
    public class SportsMigrationTests
    {
        private readonly string DbName = "GymnastikaForTests.sdf";
        private readonly string DbFolder = Directory.GetCurrentDirectory();
        private IUnityContainer _container;
        private UnityServiceLocator _serviceLocator;

        [SetUp]
        public void SetUp()
        {
            string dbPath = Path.Combine(DbFolder, DbName);
            if (File.Exists(dbPath)) File.Delete(dbPath);

            _container = new UnityContainer();
            _container
                .RegisterType<IDataServicesProviderFactory, SqlCeDataServicesProviderFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<ISessionLocator, SessionLocator>(new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionManager, TransactionManager>()
                .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                .RegisterType(typeof(IRepository<>), typeof(Repository<>))
                .RegisterType<IWorkEnvironment, WorkEnvironment>(new ContainerControlledLifetimeManager());

            _serviceLocator = new UnityServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => _serviceLocator);
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }



    }
}
