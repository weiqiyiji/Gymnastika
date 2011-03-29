
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_DietPlanTasks_20110327155234 : IDataMigration
    {
        public string TableName 
        { 
            get { return "DietPlanTasks"; }
        }
            
        public string Version 
        { 
            get { return "20110327155234"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            SchemaBuilder.CreateTable(
                TableName,
                c => c.Column<int>("Id", x => x.PrimaryKey().Identity())
                    .Column<int>("Year")
                    .Column<int>("Month")
                    .Column<int>("Day")
                    .Column<int>("SubDietPlanId")
                    .Column<int>("TaskId"));
        }
            
        public void Down()
        {
            throw new NotImplementedException();
        }
    }
}
  