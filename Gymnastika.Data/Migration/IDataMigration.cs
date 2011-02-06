using Gymnastika.Data.Migration.Commands;
using System.Collections.Generic;

namespace Gymnastika.Data.Migration 
{
    public interface IDataMigration 
    {
        SchemaBuilder SchemaBuilder { get; set; }
    }
}
