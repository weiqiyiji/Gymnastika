
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_ScheduleItems_20110325155140 : IDataMigration
    {
        public string TableName 
        { 
            get { return "ScheduleItems"; }
        }
            
        public string Version 
        { 
            get { return "20110325155140"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName, 
                x => x.Column<int>("Id", c => c.PrimaryKey().Identity())
                      .Column<int>("UserId")
                      .Column<int>("ConnectionId")
                      .Column<string>("Message"));
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  