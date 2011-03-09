using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Sport_SportsCategory_Relations_20110221202858 : IDataMigration
    {
        public const string Sports_FK       = "Sports_FK";
        public const string Category_FK     = "Category_FK";
        public const string SportId         = "SportId";
        public const string CategoryId      = "SportsCategoryId";
        public const string SportsTable     = "Sports";
        public const string CategoriesTable = "SportsCategories";


        public string TableName
        {
            get { return "Sport_SportsCategory_Relations"; }
        }

        public string Version
        {
            get { return "20110221202858"; }
        }

        public SchemaBuilder SchemaBuilder { get; set; }

        public void Up()
        {

            SchemaBuilder.CreateTable(TableName, t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                                                       .Column<int>(SportId)
                                                       .Column<int>(CategoryId));

            SchemaBuilder.CreateForeignKey(Sports_FK, TableName, new string[] { SportId }, SportsTable, new string[] { "Id" })
                         .CreateForeignKey(Category_FK, TableName, new string[] { CategoryId }, CategoriesTable, new string[] { "Id" });
        }

        public void Down()
        {
            SchemaBuilder.DropTable(TableName);

            SchemaBuilder.DropForeignKey(TableName,Sports_FK);
            SchemaBuilder.DropForeignKey(TableName,Category_FK);
        }
    }
}
