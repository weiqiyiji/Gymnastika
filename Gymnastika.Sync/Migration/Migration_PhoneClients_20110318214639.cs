
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_PhoneClients_20110318214639 : IDataMigration
    {
        public string TableName 
        { 
            get { return "PhoneClients"; }
        }
            
        public string Version 
        { 
            get { return "20110318214639"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(TableName, 
                c => c.Column<int>("Id", x => x.PrimaryKey().Identity())
                      .Column<string>("Uri"));
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  