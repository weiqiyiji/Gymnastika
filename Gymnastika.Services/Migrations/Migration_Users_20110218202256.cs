
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Users_20110218202256 : IDataMigration
    {
        public string TableName 
        { 
            get { return "Users"; }
        }
            
        public string Version 
        { 
            get { return "20110218202256"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder
                //.AlterTable(TableName, x => x.CreateIndex("IX_Users", "Id"))
                .AlterTable(TableName, x => x.AddColumn<string>("AvatarPath"));
        }
            
        public void Down()
        {
            SchemaBuilder
                //.AlterTable(TableName, x => x.DropIndex("IX_Users"))
                .AlterTable(TableName, x => x.DropColumn("AvatarPath"));
        }
    }
}
  