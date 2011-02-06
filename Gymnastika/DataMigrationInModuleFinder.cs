using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;
using Microsoft.Practices.Prism.Modularity;
using System.Reflection;

namespace Gymnastika
{
    public class DataMigrationInModuleFinder : IDataMigrationFinder
    {
        private IModuleCatalog _moduleCatalog;

        public DataMigrationInModuleFinder(IModuleCatalog catalog)
        {
            _moduleCatalog = catalog;
        }

        #region IDataMigrationFinder Members

        public IEnumerable<IDataMigration> Find()
        {
            IList<IDataMigration> dataMigrations = new List<IDataMigration>();

            foreach (ModuleInfo mi in _moduleCatalog.Modules)
            {
                Assembly assembly = Assembly.GetAssembly(mi.ModuleType);
                IEnumerable<IDataMigration> migrations = assembly.GetExportedTypes().Where(
                    t => t.GetInterface("Gymnastika.Data.Migration.IDataMigration") != null);
                
                foreach(IDataMigration dm in migrations)
                    dataMigrations.Add(dm);
            }

            return dataMigrations;
        }

        #endregion
    }
}
