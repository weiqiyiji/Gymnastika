using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Gymnastika.Data.Migration
{
    public class DataMigrationLoader : IMigrationLoader
    {
        #region IMigrationLoader Members

        public IEnumerable<IDataMigration> Load()
        {
            return Assembly.GetAssembly(this.GetType())
                        .GetExportedTypes()
                        .Where(t => t.GetInterface("Gymnastika.Data.Migration.IDataMigration") != null)
                        .Select(t => (IDataMigration)Activator.CreateInstance(t));
        }

        #endregion
    }
}
