
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_WidgetInstances_20110312155044 : IDataMigration
    {
        public string TableName 
        { 
            get { return "WidgetInstances"; }
        }
            
        public string Version 
        { 
            get { return "20110312155044"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder
                .CreateTable(TableName,
                    c => c.Column<int>("Id", x => x.PrimaryKey().Identity())
                          .Column<string>("DisplayName")
                          .Column<string>("Icon")
                          .Column<string>("Type")
                          .Column<double>("X")
                          .Column<double>("Y")
                          .Column<bool>("IsActive")
                          .Column<int>("UserId"))
                .CreateForeignKey("FK_Users", TableName, new string[] { "UserId" }, "Users", new string[] { "Id" });
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  