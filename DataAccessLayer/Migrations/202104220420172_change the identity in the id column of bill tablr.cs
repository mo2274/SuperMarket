namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetheidentityintheidcolumnofbilltablr : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "BillId", "dbo.Bills");
            DropPrimaryKey("dbo.Bills");
            AlterColumn("dbo.Bills", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Bills", "Id");
            AddForeignKey("dbo.Items", "BillId", "dbo.Bills", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "BillId", "dbo.Bills");
            DropPrimaryKey("dbo.Bills");
            AlterColumn("dbo.Bills", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Bills", "Id");
            AddForeignKey("dbo.Items", "BillId", "dbo.Bills", "Id", cascadeDelete: true);
        }
    }
}
