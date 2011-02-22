using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_NutritiveElements_20110221121551 : IDataMigration
    {
        private const string ForeignKeyName = "FK_Foods_NutritiveElements";

        public string TableName 
        { 
            get { return "NutritiveElements"; }
        }
            
        public string Version 
        { 
            get { return "20110221121551"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Name")
                    .Column<string>("Unit")
                    .Column<int>("Content")
                    .Column<int>("FoodId"));

            SchemaBuilder.CreateForeignKey(
                ForeignKeyName,
                TableName,
                new string[1] { "FoodId" },
                "Foods",
                new string[1] { "Id" });
        }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyName);
        }
    }
}
  