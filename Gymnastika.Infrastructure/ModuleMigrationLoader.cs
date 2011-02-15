using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Gymnastika.Data.Migration;
using Gymnastika.Common.Extensions;
using System.Reflection;

namespace Gymnastika
{
    public class ModuleMigrationLoader : IMigrationLoader
    {
        private IModuleCatalog _moduleCatalog;

        public ModuleMigrationLoader(IModuleCatalog catalog)
        {
            _moduleCatalog = catalog;
        }

        public IEnumerable<IDataMigration> Load()
        {
            IList<IDataMigration> dataMigrations = new List<IDataMigration>();

            foreach (ModuleInfo mi in _moduleCatalog.Modules)
            {
                Assembly assembly = Assembly.GetAssembly(Type.GetType(mi.ModuleType));
                IEnumerable<IDataMigration> migrations = 
                    assembly.GetExportedTypes()
                            .Where(t => t.GetInterface("Gymnastika.Data.Migration.IDataMigration") != null)
                            .Select(t => (IDataMigration)Activator.CreateInstance(t));

                dataMigrations.AddRange(migrations);
            }

            return dataMigrations;
        }
    }
}
