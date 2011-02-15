using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;
using System.Reflection;
using Gymnastika.Data.Tests.MockMigration;

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
            yield return new Migration_TestTables_00000000000000();
            yield return new Migration_AnotherTestTables_00000000000001();
        }

        #endregion
    }
}
