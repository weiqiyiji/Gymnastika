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
        private readonly string DbFilePath = Path.Combine("/", "GymnastikaTests.sdf");

        [Test]
        public void EnsureProviderName()
        {
            Assert.That(SqlCeDataServicesProvider.ProviderName, Is.EqualTo("SqlCe"));
        }

        [Test]
        public void DatabaseExistsAfterConfiguration()
        {
            DeleteDatabase();

            SqlCeDataServicesProvider provider = new SqlCeDataServicesProvider("/", "GymnastikaTests.sdf")
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
            SqlCeDataServicesProvider provider = new SqlCeDataServicesProvider("/", "GymnastikaTests.sdf")
            {
                AutomappingConfigurer = new MockAutomappingConfigurer()
            };

            NHibernate.Cfg.Configuration cfg = provider.BuildConfiguration(
                new DataServiceParameters { CreateDatabase = EnsureDatabase() });

            Assert.That(cfg.ClassMappings.Count, Is.EqualTo(1));

            var classMapping = cfg.ClassMappings.First();

            Assert.That(classMapping.IdentifierProperty.Name, Is.EqualTo("Id"));
            Assert.That(classMapping.EntityName, Is.StringContaining("TestTable"));
            Assert.That(classMapping.Table.Name, Is.EqualTo("TestTables"));
        }

        [TestFixtureTearDown]
        public void GlobalTearDown()
        {
            DeleteDatabase();
        }

        private bool EnsureDatabase()
        {
            return File.Exists(DbFilePath);
        }

        private void DeleteDatabase()
        {
            if (EnsureDatabase())
            {
                File.Delete(DbFilePath);
            }
        }
    }
}
