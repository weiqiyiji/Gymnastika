using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;
using System.Reflection;

namespace Gymnastika.Data.Tests.Mocks
{
    public class MockMigrationLoader : IMigrationLoader
    {
        public int MigrationCount
        {
            get
            {
                return Load().Count();
            }
        }

        #region IMigrationLoader Members

        public IEnumerable<IDataMigration> Load()
        {
            return Assembly.GetAssembly(this.GetType()).GetExportedTypes()
                .Where(t => t.GetInterface("Gymnastika.Data.Migration.IDataMigration") != null)
                .Select(t => (IDataMigration)Activator.CreateInstance(t));
        }

        #endregion
    }
}
