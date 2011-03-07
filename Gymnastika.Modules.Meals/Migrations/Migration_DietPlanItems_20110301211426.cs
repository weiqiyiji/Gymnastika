
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_DietPlanItems_20110301211426 : IDataMigration
    {
        private const string ForeignKeyName = "FK_SubDietPlans_DietPlanItems";

        public string TableName 
        { 
            get { return "DietPlanItems"; }
        }
            
        public string Version 
        { 
            get { return "20110301211426"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<decimal>("Amount")
                    .Column<int>("SubDietPlanId"));

            SchemaBuilder.CreateForeignKey(
                ForeignKeyName,
                TableName,
                new string[1] { "SubDietPlanId" },
                "SubDietPlans",
                new string[1] { "Id" });
        }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyName);
        }
    }
}
  