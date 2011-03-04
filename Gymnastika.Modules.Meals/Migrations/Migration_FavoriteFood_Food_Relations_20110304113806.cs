
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_FavoriteFood_Food_Relations_20110304113806 : IDataMigration
    {
        private const string ForeignKeyNameWithFavoriteFoods = "FK_FavoriteFoods_FavoriteFood_Food_Relations";
        private const string ForeignKeyNameWithFoods = "FK_Foods_FavoriteFood_Food_Relations";

        public string TableName 
        { 
            get { return "FavoriteFood_Food_Relations"; }
        }
            
        public string Version 
        { 
            get { return "20110304113806"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<int>("FavoriteFoodId")
                    .Column<int>("FoodId"));

            SchemaBuilder.CreateForeignKey(
                ForeignKeyNameWithFavoriteFoods,
                TableName,
                new string[] { "FavoriteFoodId" },
                "FavoriteFoods",
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

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyNameWithFavoriteFoods);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyNameWithFoods);
        }
    }
}
  