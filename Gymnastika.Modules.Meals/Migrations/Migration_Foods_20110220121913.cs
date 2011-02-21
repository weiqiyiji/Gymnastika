
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Foods_20110220121913 : IDataMigration
    {
        private const string ForeignKeyNameWithSubCategories = "FK_Foods_SubCategories";
        private const string ForeignKeyNameWithSubDietPlans = "FK_Foods_SubDietPlans";

        public string TableName 
        { 
            get { return "Foods"; }
        }
            
        public string Version 
        { 
            get { return "20110220121913"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Name")
                    .Column<string>("SmallImageUri")
                    .Column<string>("LargeImageUri")
                    .Column<int>("Calorie")
                    .Column<string>("Introduction")
                    .Column<string>("NutritionalValue")
                    .Column<string>("Function")
                    .Column<string>("SuitableCrowd")
                    .Column<int>("SubCategoryId"));

            SchemaBuilder.CreateForeignKey(
                ForeignKeyNameWithSubCategories,
                TableName,
                new string[1] { "SubCategoryId" },
                "SubCategories",
                new string[1] { "Id" });

            SchemaBuilder.CreateForeignKey(
                ForeignKeyNameWithSubDietPlans,
                TableName,
                new string[1] { "SubDietPlanId" },
                "SubDietPlans",
                new string[1] { "Id" });
        }
        
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyNameWithSubCategories);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyNameWithSubDietPlans);
        }
    }
}
  