using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Services.Migrations
{
    public class Migration_Users_20110215161221 : IDataMigration
    {
        public string TableName 
        { 
            get { return "Users"; }
        }
            
        public string Version 
        { 
            get
            {
                return "20110215161221"; 
            }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName, 
                t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                      .Column<string>("UserName")
                      .Column<string>("Password")
                      .Column<int>("Gender")
                      .Column<int>("Age")
                      .Column<int>("Height")
                      .Column<int>("Weight")
                      .Column<bool>("IsActive"));
        }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);
        }
    }
}
  