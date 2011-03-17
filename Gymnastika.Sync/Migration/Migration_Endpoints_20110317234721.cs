
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Endpoints_20110317234721 : IDataMigration
    {
        public string TableName 
        { 
            get { return "Endpoints"; }
        }
            
        public string Version 
        { 
            get { return "20110317234721"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.AlterTable(TableName, x => x.AddColumn<string>("Type"));
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  