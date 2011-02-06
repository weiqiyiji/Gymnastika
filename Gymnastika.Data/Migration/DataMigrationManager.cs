using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gymnastika.Common.Logging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Gymnastika.Data.Migration
{
    public class DataMigrationManager : IDataMigrationManager 
    {
        private SchemaBuilder _schemaBuilder;
        private IDataMigrationFinder _migrationFinder;

        public DataMigrationManager(
            SchemaBuilder schemaBuilder, IDataMigrationFinder migrationFinder, ILogger logger)
        {
            _schemaBuilder = schemaBuilder;
            _migrationFinder = migrationFinder;
            DataMigrations = new List<IDataMigration>(migrationFinder.Find());
            Logger = logger;
        }

        public ILogger Logger { get; set; }

        private static Tuple<int, MethodInfo> GetUpdateMethod(IDataMigration migration)
        {
            const string updatefromPrefix = "UpdateFrom";

            MethodInfo mi = 
                migration.GetType().GetMethods().FirstOrDefault(m => m.Name.StartsWith(updatefromPrefix));

            if (mi != null) 
            {
                var version = mi.Name.Substring(updatefromPrefix.Length);
                int versionValue;
                if (int.TryParse(version, out versionValue)) 
                {
                    return new Tuple<int, MethodInfo>(versionValue, mi);
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the Create method from a data migration class if it's found
        /// </summary>
        private static MethodInfo GetCreateMethod(IDataMigration dataMigration) 
        {
            var methodInfo = dataMigration.GetType().GetMethod("Create", BindingFlags.Public | BindingFlags.Instance);
            if(methodInfo != null && methodInfo.ReturnType == typeof(int)) 
            {
                return methodInfo;
            }

            return null;
        }

        /// <summary>
        /// Returns the Uninstall method from a data migration class if it's found
        /// </summary>
        private static MethodInfo GetUninstallMethod(IDataMigration dataMigration) 
        {
            var methodInfo = dataMigration.GetType().GetMethod("Uninstall", BindingFlags.Public | BindingFlags.Instance);
            if ( methodInfo != null && methodInfo.ReturnType == typeof(void) ) 
            {
                return methodInfo;
            }

            return null;
        }

        #region IDataMigrationManager Members

        public void Migrate()
        {
            foreach(IDataMigration migration in DataMigrations)
            {
                migration.SchemaBuilder = _schemaBuilder;
                MethodInfo mi = GetCreateMethod(migration);
                if (mi != null)
                {
                    mi.Invoke(mi, new object[0]);
                    continue;
                }

                Tuple<int, MethodInfo> tuple = GetUpdateMethod(migration);
                if (tuple != null)
                {
                    tuple.Item2.Invoke(tuple.Item2, new object[0]);
                    continue;
                }

                mi = GetUninstallMethod(migration);
                if (mi != null)
                {
                    mi.Invoke(mi, new object[0]);
                    continue;
                }
            }

            DataMigrations.Clear();
        }

        public IList<IDataMigration> DataMigrations { get; set; }

        #endregion
    }
}
