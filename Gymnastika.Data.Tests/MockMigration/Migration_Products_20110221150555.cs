
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Data.Tests.MockMigration
{
    public class Migration_Products_20110221150555 : IDataMigration
    {
        public string TableName 
        { 
            get { return "Products"; }
        }
            
        public string Version 
        { 
            get { return "20110221150555"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(TableName,
                x => x.Column<int>("Id", c => c.PrimaryKey().Identity())
                      .Column<string>("Name")
                );
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  