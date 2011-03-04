
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Introductions_20110223125751 : IDataMigration
    {
        private const string ForeignKeyName = "FK_Foods_Introductions";

        public string TableName 
        { 
            get { return "Introductions"; }
        }
            
        public string Version 
        { 
            get { return "20110223125751"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }

        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Name")
                    .Column<string>("Content")
                    .Column<int>("FoodId"));

            SchemaBuilder.CreateForeignKey(
                ForeignKeyName,
                TableName,
                new string[] { "FoodId" },
                "Foods",
                new string[] { "Id" });
        }

        public void Down()
        {
            SchemaBuilder.DropTable(TableName);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyName);
        }
    }
}
  