
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_Connections_20110318215110 : IDataMigration
    {
        public string TableName 
        { 
            get { return "Connections"; }
        }
            
        public string Version 
        { 
            get { return "20110318215110"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(TableName,
                c => c.Column<int>("Id", x => x.PrimaryKey().Identity())
                      .Column<int>("DesktopClientId")
                      .Column<int>("PhoneClientId"));

            SchemaBuilder.CreateForeignKey(
                "FK_Source_DesktopClients",
                TableName, new string[] { "DesktopClientId" },
                "DesktopClients", new string[] { "Id" });

            SchemaBuilder.CreateForeignKey(
                "FK_Target_PhoneClients",
                TableName, new string[] { "PhoneClientId" },
                "PhoneClients", new string[] { "Id" });
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  