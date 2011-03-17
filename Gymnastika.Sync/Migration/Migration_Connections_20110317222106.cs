
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Connections_20110317222106 : IDataMigration
    {
        public string TableName 
        { 
            get { return "Connections"; }
        }
            
        public string Version 
        { 
            get { return "20110317222106"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }

        private const string EndpointTableName = "Endpoints";
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName, c => c.Column<int>("Id", x => x.PrimaryKey().Identity())
                                 .Column<int>("SourceId")
                                 .Column<int>("TargetId"));

            SchemaBuilder.CreateForeignKey(
                "FK_Connection_Source", 
                TableName, new string[] { "SourceId" }, 
                EndpointTableName, new string[] { "Id" });

            SchemaBuilder.CreateForeignKey(
                "FK_Connection_Target",
                TableName, new string[] { "TargetId" },
                EndpointTableName, new string[] { "Id" });
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  