namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumndate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "Date", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "Date");
        }
    }
}
