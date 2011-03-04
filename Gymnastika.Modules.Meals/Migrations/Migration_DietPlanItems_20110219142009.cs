
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_DietPlanItems_20110219142009 : IDataMigration
    {
        private const string ForeignKeyNameWithSubDietPlans = "FK_SubDietPlans_DietPlanItems";
        private const string ForeignKeyNameWithFoods = "FK_Foods_DietPlanItems";

        public string TableName 
        { 
            get { return "DietPlanItems"; }
        }
            
        public string Version 
        { 
            get { return "20110219142009"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<decimal>("Amount")
                    .Column<int>("SubDietPlanId")
                    .Column<int>("FoodId"));

            SchemaBuilder.CreateForeignKey(
                ForeignKeyNameWithSubDietPlans,
                TableName,
                new string[] { "SubDietPlanId" },
                "SubDietPlans",
                new string[] { "Id" });

            SchemaBuilder.CreateForeignKey(
                ForeignKeyNameWithFoods,
                TableName,
                new string[] { "FoodId" },
                "Foods",
                new string[] { "Id" });
        }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyNameWithSubDietPlans);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyNameWithFoods);
        }
    }
}
  