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
using Gymnastika.Common.Logging;
using Gymnastika.Tests.Support;
using Gymnastika.Data.Migration.Interpreters;
using Gymnastika.Common.Configuration;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Gymnastika.Data.Models;
using NHibernate;
using Gymnastika.Migrations;

namespace Gymnastika.Modules.Sports.Tests
{
    #region Mocks
    public class MockAutomappingConfigurer : IAutomappingConfigurer
    {
        #region IAutomappingConfigurer Members

        public System.Collections.Generic.IEnumerable<AutomappingConfigurationMetadata> GetAutomappingMetadata()
        {
            yield return new AutomappingConfigurationMetadata() { AssemblyName = "Gymnastika.Data.dll" };
            yield return new AutomappingConfigurationMetadata() { AssemblyName = "Gymnastika.Modules.Sports.Tests.dll" };
            yield return new AutomappingConfigurationMetadata() { AssemblyName = "Gymnastika.Modules.Sports.dll" };
        }

        #endregion
    }

    #endregion

    [TestFixture]
    public class SportsMigrationTests
    {
        private readonly string DbName = "GymnastikaForTests.sdf";
        private readonly string LogFileName = "logs.txt";
        private readonly string DbFolder = Directory.GetCurrentDirectory();
        private IUnityContainer _container;
        private UnityServiceLocator _serviceLocator;
        ILogger logger = new FileLogger();
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }


        public void MigrateTablesInSportsModules()
        {
            IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().GetWorkContextScope();

            var migrationManager = _container.Resolve<IDataMigrationManager>();
            migrationManager.Migrate();
            
            scope.Dispose();
        }

        [Test]
        public void InsertRecordsIntoTables()
        {
            var sportRepository = _container.Resolve<IRepository<Sport>>();
            var categoryRepository = _container.Resolve<IRepository<SportsCategory>>();
            var itemRepository = _container.Resolve<IRepository<SportsPlanItem>>();
            var planRepository = _container.Resolve<IRepository<SportsPlan>>();

            IWorkContextScope scope = _container.Resolve<IWorkEnvironment>().GetWorkContextScope();

            Sport firstSport = new Sport() { Name = "Sport1" };
            sportRepository.Create(firstSport);

            var sports = sportRepository.Fetch(t => true).ToList();
            Assert.AreEqual(1, sports.Count);

            SportsCategory firstCategory = new SportsCategory() { Name = "Category1", Sports =  sports};
            categoryRepository.Create(firstCategory);
            

            Assert.AreEqual(firstSport.Name, categoryRepository.Get(1).Sports.First().Name);
            //Assert.AreEqual(firstCategory.Name, sportRepository.Get(1).SportsCategories.First().Name);
            
            scope.Dispose();  
        }
    }
}
