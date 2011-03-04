using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_SportsPlans_20110220115124 : IDataMigration
    {
        public string TableName 
        { 
            get { return "SportsPlans"; }
        }
            
        public string Version 
        { 
            get { return "20110220115124"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(TableName, t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                                                       .Column<DateTime>("Time")
                                                       .Column<int>("Score"));
       }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);
        }
    }
}
  