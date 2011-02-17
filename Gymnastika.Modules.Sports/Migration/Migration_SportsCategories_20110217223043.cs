
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_SportsCategories_20110217223043 : IDataMigration
    {
        public string TableName 
        { 
            get { return "SportsCategories"; }
        }
            
        public string Version 
        {
            get { return "20110217223043"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(TableName, t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                                                       .Column<string>("Name")
                                                       .Column<string>("ImageUri"));
        }

        public void Down()
        {
            SchemaBuilder.DropTable(TableName);
        }
    }
}
  