
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_SubDietPlans_20110219142008 : IDataMigration
    {
        private const string ForeignKeyName = "FK_DietPlans_SubDietPlans";

        public string TableName 
        { 
            get { return "SubDietPlans"; }
        }
            
        public string Version 
        { 
            get { return "20110219142008"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Name")
                    .Column<int>("DietPlanId"));

            SchemaBuilder.CreateForeignKey(
                ForeignKeyName,
                TableName,
                new string[] { "DietPlanId" },
                "DietPlans",
                new string[] { "Id" });
        }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyName);
        }
    }
}
  