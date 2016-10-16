namespace todoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inclusao_username_tarefa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tarefas", "Username", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tarefas", "Username");
        }
    }
}
