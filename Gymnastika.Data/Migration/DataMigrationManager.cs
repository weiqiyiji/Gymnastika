using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gymnastika.Common.Logging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Data.Migration.Interpreters;
using Gymnastika.Data.SessionManagement;
using Gymnastika.Common.Extensions;
using NHibernate;
using NHibernate.Linq;
using Gymnastika.Data.Models;
using Gymnastika.Common.Utils;
using System.Threading;

namespace Gymnastika.Data.Migration
{
    public class DataMigrationManager : IDataMigrationManager
    {
        private ISessionLocator _sessionLocator;
        private IDataMigrationInterpreter _interpreter;
        private IRepository<MigrationRecord> _migrationRecordRepository;
        private IMigrationLoader[] _migrationLoaders;

        public DataMigrationManager(
            IMigrationLoader[] migrationLoaders,
            ISessionLocator sessionLocator,
            IDataMigrationInterpreter interpreter,
            IRepository<MigrationRecord> migrationRecordRepository,
            ILogger logger)
        {
            _migrationLoaders = migrationLoaders;
            _sessionLocator = sessionLocator;
            _interpreter = interpreter;
            _migrationRecordRepository = migrationRecordRepository;
            Logger = logger;
        }

        public ILogger Logger { get; set; }

        protected IEnumerable<IDataMigration> LoadDataMigrations()
        {
            IList<IDataMigration> foundMigrations = new List<IDataMigration>();

            foreach (IMigrationLoader loader in _migrationLoaders)
            {
                foundMigrations.AddRange(loader.Load());
            }

            return foundMigrations;
        }

        public void EnsureMigrationRecordsExists()
        {
            try
            {
                Logger.Debug("DataMigrationManager", "Check the table MigrationRecords exists");
                //Check whether the table already existed
                _migrationRecordRepository.Fetch(m => true);
            }
            catch(Exception e)
            {
                Logger.Debug("DataMigrationManager", "MigrationRecords does not exist");
                SchemaBuilder builder = new SchemaBuilder(_interpreter);

                builder.CreateTable("MigrationRecords",
                    t => t.Column<int>("Id", column => column.PrimaryKey().Identity())
                          .Column<string>("Version", column => column.WithLength(16))
                          .Column<string>("TableName"));
            }
        }

        #region IDataMigrationManager Members

        public void Migrate()
        {
            EnsureMigrationRecordsExists();

            IEnumerable<IDataMigration> migrations = LoadDataMigrations();

            IEnumerable<MigrationRecord> migrationRecords = _migrationRecordRepository.Fetch(m => true);
            string maxRecordVersion = migrationRecords.Max(m => m.Version);

            IEnumerable<IDataMigration> newMigrations =
                migrations.Where(m => m.Version.CompareTo(maxRecordVersion) > 0).OrderBy(m => m.Version);

            foreach (IDataMigration migration in newMigrations)
            {
                migration.SchemaBuilder = new SchemaBuilder(_interpreter);
                migration.Up();

                DoUpdateMigrationRecord(migration);
            }
        }

        public void Migrate(string version)
        {
            EnsureMigrationRecordsExists();

            IEnumerable<IDataMigration> migrations = LoadDataMigrations();
            IDataMigration migration = migrations.SingleOrDefault(m => m.Version == version);

            if (migration == null)
                throw new MigrationNullException(version);

            IEnumerable<IDataMigration> readyToUndoMigrations =
                    migrations.Where(m => m.TableName == migration.TableName
                                        && m.Version.CompareTo(version) > 0)
                              .OrderByDescending(m => m.Version);

            foreach (IDataMigration m in readyToUndoMigrations)
            {
                m.SchemaBuilder = new SchemaBuilder(_interpreter);
                m.Down();
            }

            DoUpdateMigrationRecord(migration);
        }

        #endregion

        protected void DoUpdateMigrationRecord(IDataMigration dataMigration)
        {
            MigrationRecord record = _migrationRecordRepository.Get(m => m.TableName == dataMigration.TableName);

            if (record == null)
            {
                record = new MigrationRecord
                {
                    TableName = dataMigration.TableName
                };
            }

            record.Version = dataMigration.Version;

            Logger.Debug("DataMigrationManager", "Migrate {0} to version:{1}", record.TableName, record.Version);
            _migrationRecordRepository.CreateOrUpdate(record);
        }
    }
}
