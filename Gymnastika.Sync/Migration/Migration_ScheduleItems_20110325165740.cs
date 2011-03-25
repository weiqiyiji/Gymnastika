
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_ScheduleItems_20110325165740 : IDataMigration
    {
        public string TableName 
        { 
            get { return "ScheduleItems"; }
        }
            
        public string Version 
        { 
            get { return "20110325165740"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.AlterTable(
                TableName, a => a.AddColumn<string>("StartTime"));
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  