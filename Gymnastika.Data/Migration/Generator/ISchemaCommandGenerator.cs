using System.Collections.Generic;
using Gymnastika.Data.Migration.Commands;

namespace Gymnastika.Data.Migration.Generator {
    public interface ISchemaCommandGenerator {
        /// <summary>
        /// Automatically updates the tables in the database.
        /// </summary>
        void UpdateDatabase();

        /// <summary>
        /// Creates the tables in the database.
        /// </summary>
        void CreateDatabase();

    }
}