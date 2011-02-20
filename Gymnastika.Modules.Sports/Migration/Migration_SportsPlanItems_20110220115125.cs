using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_SportsPlanItems_20110220115125 : IDataMigration
    {
        public string TableName 
        { 
            get { return "SportsPlanItems"; }
        }
            
        public string Version 
        { 
            get { return "20110220115125"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(TableName,t=>t.Column<int>("Id",c=>c.PrimaryKey().Identity())
                                                    .Column<int>("SportsTime_Hour")
                                                    .Column<int>("SportsTime_Min")
                                                    .Column<int>("Duration")
                                                    .Column<bool>("Completed")
                                                    .Column<int>("SportId"));
        }

        public void Down()
        {
            SchemaBuilder.DropTable(TableName);
        }
    }
}
  