using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_DietPlans_20110217121623 : IDataMigration
    {
        //private const string ForeignKeyName = "FK_UserDietPlanMappings_DietPlans";
        private const string ForeignKeyName = "FK_Users_DietPlans";
        public string TableName 
        { 
            get { return "DietPlans"; }
        }
            
        public string Version 
        { 
            get { return "20110217121623"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Name")
                    .Column<DateTime>("CreatedDate")
                    .Column<bool>("PlanType")
                    .Column<int>("UserId"));

            //SchemaBuilder.CreateForeignKey(
            //    ForeignKeyName,
            //    TableName,
            //    new string[] { "UserId" },
            //    "UserDietPlanMappings",
            //    new string[] { "Id" });

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
  