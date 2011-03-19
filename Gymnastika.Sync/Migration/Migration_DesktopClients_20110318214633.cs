
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_DesktopClients_20110318214633 : IDataMigration
    {
        public string TableName 
        { 
            get { return "DesktopClients"; }
        }
            
        public string Version 
        { 
            get { return "20110318214633"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(TableName, 
                c => c.Column<int>("Id", x => x.PrimaryKey().Identity())
                      .Column<string>("Placeholder"));
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  