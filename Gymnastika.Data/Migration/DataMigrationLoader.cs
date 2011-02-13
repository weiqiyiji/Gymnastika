using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data.Migration
{
    public class DataMigrationLoader : IMigrationLoader
    {
        #region IMigrationLoader Members

        public IEnumerable<IDataMigration> Load()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
