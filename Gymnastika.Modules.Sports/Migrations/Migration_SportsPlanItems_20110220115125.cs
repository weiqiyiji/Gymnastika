using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_SportsPlanItems_20110220115125 : IDataMigration
    {
        public const string Sports_FK = "Sports_FK";
        public const string SportsTable = "Sports";
        public const string Plans_FK = "SportsPlans_FK";

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
            SchemaBuilder.CreateTable(TableName, t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                                                    .Column<DateTime>("Time")
                                                    .Column<int>("Duration")
                                                    .Column<bool>("Completed")
                                                    .Column<int>("SportId")
                                                    .Column<int>("SportsPlanId"));

            SchemaBuilder.CreateForeignKey(Sports_FK, TableName, new string[] { "SportId" }, SportsTable, new string[] { "Id" })
                         .CreateForeignKey(Plans_FK, TableName, new string[] { "SportsPlanId" }, "SportsPlans", new string[] { "Id" });
        }

        public void Down()
        {
            SchemaBuilder.DropTable(TableName);

            SchemaBuilder.DropForeignKey(TableName,Sports_FK);
            SchemaBuilder.DropForeignKey(TableName, Plans_FK);
        }
    }
}
  