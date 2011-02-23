
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Food_SubDietPlan_Relations_20110222133930 : IDataMigration
    {
        private const string ForeignKeyNameWithFoods = "FK_Foods_Food_SubDietPlan_Relations";
        private const string ForeignKeyNameWithSubCategories = "FK_SubCategories_Food_SubDietPlan_Relations";

        public string TableName 
        { 
            get { return "Food_SubDietPlan_Relations"; }
        }
            
        public string Version 
        { 
            get { return "20110222133930"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(TableName,
                x => x.Column<int>("Id")
                      .Column<int>("FoodId")
                      .Column<int>("SubCategoryId"));

            SchemaBuilder.CreateForeignKey(
                ForeignKeyNameWithFoods, 
                TableName, 
                new string[1] { "FoodId" }, 
                "Foods", 
                new string[1] { "Id" });

            SchemaBuilder.CreateForeignKey(
                ForeignKeyNameWithSubCategories, 
                TableName, 
                new string[1] { "SubCategoryId" }, 
                "SubCategorys", 
                new string[1] { "Id" });
        }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyNameWithFoods);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyNameWithSubCategories);
        }
    }
}
  