using System.Collections.Generic;

namespace Gymnastika.Data.Migration 
{
    public interface IDataMigrationManager 
    {
        void Migrate();
        IList<IDataMigration> DataMigrations { get; set; }
    }
}