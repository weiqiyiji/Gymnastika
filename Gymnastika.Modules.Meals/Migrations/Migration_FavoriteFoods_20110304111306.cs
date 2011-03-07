
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_FavoriteFoods_20110304111306 : IDataMigration
    {
        private const string ForeignKeyName = "FK_Users_FavoriteFoods";

        public string TableName 
        { 
            get { return "FavoriteFoods"; }
        }
            
        public string Version 
        { 
            get { return "20110304111306"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<int>("UserId"));

            SchemaBuilder.CreateForeignKey(
                ForeignKeyName,
                TableName,
                new string[] { "UserId" },
                "Users",
                new string[] { "Id" });
        }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);

            SchemaBuilder.DropForeignKey(TableName, ForeignKeyName);
        }
    }
}
  