using Gymnastika.Data.Migration.Commands;
using System.Collections.Generic;

namespace Gymnastika.Data.Migration 
{
    public interface IDataMigration 
    {
        string Version { get; }
        string TableName { get; }
        SchemaBuilder SchemaBuilder { get; set; }

        void Up();
        void Down();
    }
}
