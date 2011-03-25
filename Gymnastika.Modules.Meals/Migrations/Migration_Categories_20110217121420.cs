using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Categories_20110217121420 : IDataMigration
    {
        public string TableName 
        { 
            get { return "Categories"; }
        }
            
        public string Version 
        { 
            get { return "20110217121420"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Name"));
        }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);
        }
    }
}
  