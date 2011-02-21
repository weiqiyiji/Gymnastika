using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data.Migration;

namespace Gymnastika.Migrations
{
    public class Migration_SportsPlans_20110220115704 : IDataMigration
    {
        public string TableName 
        { 
            get { return "SportsPlans"; }
        }
            
        public string Version 
        { 
            get { return "20110220115704"; }
        }
            
        public SchemaBuilder SchemaBuilder { get; set; }
            
        public void Up()
        {
            #region definition
            //public class SportsPlan
                //{
                //    public virtual int Id { set; get; }

                //    public virtual int Year { get; set; }

                //    public virtual int Month { get; set; }

                //    public virtual int Day { get; set; }

                //    public virtual int Score { get; set; }

                //    public virtual IList<SportsPlanItem> SportsPlanItems { get; set; }
            //}
            #endregion

            SchemaBuilder.CreateTable(TableName, t => t.Column<int>("Id", c => c.PrimaryKey().Identity())
                                                    .Column<int>("Year")
                                                    .Column<int>("Month")
                                                    .Column<int>("Day")
                                                    .Column<int>("Score"));
       }
            
        public void Down()
        {
            SchemaBuilder.DropTable(TableName);
        }
    }
}
  