using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Data.Tests.MockMigration
{
    public class Migration_TestTables_00000000000000 : IDataMigration
    {
        #region IDataMigration Members

        public string Version
        {
            get { return "00000000000000"; }
        }

        public string TableName
        {
            get { return "TestTables"; }
        }

        public SchemaBuilder SchemaBuilder { get; set; }

        #endregion

        public void Up()
        {
            SchemaBuilder.CreateTable("TestTables",
                t => t.Column<int>("Id")
                      .Column<string>("TableName"));
        }

        public void Down()
        {
            SchemaBuilder.DropTable("TestTables");
        }
    }
}
