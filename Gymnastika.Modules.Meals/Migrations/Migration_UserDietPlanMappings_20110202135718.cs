
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_UserDietPlanMappings_20110222135718 : IDataMigration
    {
        public string TableName 
        { 
            get { return "UserDietPlanMappings"; }
        }
            
        public string Version 
        { 
            get { return "20110202135718"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey()));
        }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);
        }
    }
}
  