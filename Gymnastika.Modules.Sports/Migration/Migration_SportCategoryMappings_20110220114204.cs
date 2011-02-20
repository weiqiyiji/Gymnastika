using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_SportCategoryMappings_20110220114204 : IDataMigration
    {
        public string TableName 
        { 
            get { return "SportCategoryMappings"; }
        }
            
        public string Version 
        { 
            get { return "20110220114204"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(TableName,t=>t.Column<int>("Id",c=>c.PrimaryKey().Identity())
                                                    .Column<int>("SportId")
                                                    .Column<int>("CategoryId"));
        }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);
        }
    }
}
  