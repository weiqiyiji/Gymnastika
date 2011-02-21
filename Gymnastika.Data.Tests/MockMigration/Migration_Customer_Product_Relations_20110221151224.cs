using System;
using Gymnastika.Data.Migration;

namespace Gymnastika.Data.Tests.MockMigration
{
    public class Migration_Customer_Product_Relations_20110221151224 : IDataMigration
    {
        public string TableName 
        { 
            get { return "Customer_Product_Relations"; }
        }
            
        public string Version 
        { 
            get { return "20110221151224"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(TableName,
                x => x.Column<int>("Id")
                      .Column<int>("CustomerId")
                      .Column<int>("ProductId"));

            SchemaBuilder.CreateForeignKey("FK_Customers_Customer_Product_Relations", TableName, new string[] { "CustomerId" }, "Customers", new string[] { "Id" });
            SchemaBuilder.CreateForeignKey("FK_Products_Customer_Product_Relations", TableName, new string[] { "ProductId" }, "Products", new string[] { "Id" });
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  