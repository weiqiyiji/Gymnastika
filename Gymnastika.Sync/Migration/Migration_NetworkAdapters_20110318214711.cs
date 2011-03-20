
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_NetworkAdapters_20110318214711 : IDataMigration
    {
        public string TableName 
        { 
            get { return "NetworkAdapters"; }
        }
            
        public string Version 
        { 
            get { return "20110318214711"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }

        private const string DesktopClientTableName = "DesktopClients";

        public void Up()
        {
            SchemaBuilder.CreateTable(TableName,
                c => c.Column<int>("Id", x => x.PrimaryKey().Identity())
                      .Column<string>("IpAddress")
                      .Column<string>("SubnetMask")
                      .Column<string>("DefaultGateway")
                      .Column<int>("DesktopClientId"));

            SchemaBuilder.CreateForeignKey(
                "FK_DesktopClients", 
                TableName, new string[] { "DesktopClientId" }, 
                DesktopClientTableName, new string[] { "Id" });
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  