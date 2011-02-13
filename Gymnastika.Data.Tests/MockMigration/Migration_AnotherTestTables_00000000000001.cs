using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Data.Tests.MockMigration
{
    public class Migration_AnotherTestTables_00000000000001 : IDataMigration
    {
        #region IDataMigration Members

        public string Version
        {
            get { return "00000000000001"; }
        }

        public string TableName
        {
            get { return "AnotherTestTables"; }
        }

        public SchemaBuilder SchemaBuilder { get; set; }

        public void Up()
        {
            SchemaBuilder.CreateTable(TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity()));
        }

        public void Down()
        {
            SchemaBuilder.DropTable(TableName);
        }

        #endregion
    }
}
