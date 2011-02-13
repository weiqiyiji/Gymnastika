using System.Collections.Generic;

namespace Gymnastika.Data.Migration 
{
    public interface IDataMigrationManager 
    {
        void Migrate();
        void Migrate(string version);
        void EnsureMigrationRecordsExists();
    }
}