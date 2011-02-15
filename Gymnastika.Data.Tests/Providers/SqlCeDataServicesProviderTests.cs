using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Gymnastika.Data.Providers;
using System.IO;
using Gymnastika.Data.Tests.Mocks;

namespace Gymnastika.Data.Tests.Providers
{
    [TestFixture]
    public class SqlCeDataServicesProviderTests
    {
        private readonly string DbName = "GymnastikaForTests.sdf";
        private readonly string CurrentDirectory = Directory.GetCurrentDirectory();
        private string _dbFilePath;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _dbFilePath = Path.Combine(CurrentDirectory, DbName);
        }

        [Test]
        public void EnsureProviderName()
        {
            Assert.That(SqlCeDataServicesProvider.ProviderName, Is.EqualTo("SqlCe"));
        }

        [Test]
        public void DatabaseExistsAfterConfiguration()
        {
            DeleteDatabase();

            SqlCeDataServicesProvider provider = new SqlCeDataServicesProvider(CurrentDirectory, DbName)
            {
                AutomappingConfigurer = new MockAutomappingConfigurer()
            };

            NHibernate.Cfg.Configuration cfg = provider.BuildConfiguration(
                new DataServiceParameters { CreateDatabase = true });

            Assert.That(EnsureDatabase(), Is.True);
        }

        [Test]
        public void ClassMappingCorrectAfterConfiguration()
        {
            var mockConfigurer = new MockAutomappingConfigurer();

            SqlCeDataServicesProvider provider = new SqlCeDataServicesProvider(CurrentDirectory, DbName)
            {
                AutomappingConfigurer = mockConfigurer
            };

            NHibernate.Cfg.Configuration cfg = provider.BuildConfiguration(
                new DataServiceParameters { CreateDatabase = EnsureDatabase() });

            Assert.That(cfg.ClassMappings.Count, Is.EqualTo(mockConfigurer.ModelCount));
        }

        [Test]
        [ExpectedException(typeof(DataServicesProviderInitializationException))]
        public void ThrowForDatabaseNameMissing()
        {
            SqlCeDataServicesProvider provider = new SqlCeDataServicesProvider("/", "");
        }

        [Test]
        [ExpectedException(typeof(DataServicesProviderInitializationException))]
        public void ThrowForDataFolderMissing()
        {
            SqlCeDataServicesProvider provider = new SqlCeDataServicesProvider("", "Null");
        }

        [Test]
        [ExpectedException(typeof(DataServicesProviderInitializationException))]
        public void ThrowForFileNameMissing()
        {
            SqlCeDataServicesProvider provider = new SqlCeDataServicesProvider("");
        }

        [TestFixtureTearDown]
        public void GlobalTearDown()
        {
            DeleteDatabase();
        }

        private bool EnsureDatabase()
        {
            return File.Exists(_dbFilePath);
        }

        private void DeleteDatabase()
        {
            if (EnsureDatabase())
            {
                File.Delete(_dbFilePath);
            }
        }
    }
}
