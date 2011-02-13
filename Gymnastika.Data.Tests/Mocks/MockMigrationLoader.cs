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
        #region IMigrationLoader Members

        public IEnumerable<IDataMigration> Load()
        {
            return Assembly.GetExecutingAssembly().GetExportedTypes()
                    .Where(t => t.GetInterface("IDataMigration") != null)
                    .Select(t => Activator.CreateInstance(t) as IDataMigration);
        }

        #endregion
    }
}
