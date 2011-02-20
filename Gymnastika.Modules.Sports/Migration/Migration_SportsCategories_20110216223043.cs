
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_SportsCategories_20110216223043 : IDataMigration
    {
        public string TableName 
        { 
            get { return "SportsCategories"; }
        }
            
        public string Version 
        {
            get { return "20110216223043"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(TableName, t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                                                       .Column<string>("Name")
                                                       .Column<string>("ImageUri")
                                                       .Column<string>("Note"));
            
            SchemaBuilder.CreateForeignKey("Sports", TableName, new string[] { "Id" }, "Sports", new string[] { "Id" });
        }

        public void Down()
        {
            SchemaBuilder.DropTable(TableName);
        }
    }
}
  