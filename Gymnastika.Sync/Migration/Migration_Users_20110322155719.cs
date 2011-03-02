
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Users_20110322155719 : IDataMigration
    {
        public string TableName 
        { 
            get { return "Users"; }
        }
            
        public string Version 
        { 
            get { return "20110322155719"; }
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
                         .Column<bool>("IsActive")
                         .Column<string>("AvatarPath"));
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  