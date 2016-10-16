using System.Data.Entity;

namespace todoList.Models
{
    public class Contexto : DbContext
    {
        public Contexto(): base ("Contexto")
        {
                
        }
        public DbSet<Tarefas> Tarefas { get; set; }

    }
}