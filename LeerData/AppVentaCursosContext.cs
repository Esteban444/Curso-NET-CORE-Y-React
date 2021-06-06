using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace LeerData
{
    public class AppVentaCursosContext: DbContext
    {
        private const string connectionString = @"Data Source=DESKTOP-2T5F1O1;Initial Catalog=CursosOnline;Persist Security Info=False;User ID=sa;Password=123;";
        //@"Data Source=DESKTOP-2T5F1O1;Initial Catalog=CursosOnline;Integrated Security=true"
          
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
               options.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<CursoInstructor>().HasKey(c => new {c.CursoId, c.InstructorId});
            //modelBuilder.HasNoKey();
        }
        public DbSet<Curso> Cursos {get; set;}
        public DbSet<Precio> Precios {get; set;}
        public DbSet<Comentario> Comentarios {get; set;}
        public DbSet<Instructor> Instructores {get; set;}
        public DbSet<CursoInstructor> CursoInstructor {get; set;}
    }
}