using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public class CursosOnlineContext: IdentityDbContext<Usuario>
    {
        public CursosOnlineContext(DbContextOptions options): base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CursoInstructor>().HasKey(c => new {c.CursoId, c.InstructorId});
        }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<CursoInstructor> CursoInstructor { get; set; }
        public DbSet<Instructor> Instructores { get; set; }
        public DbSet<Precio> Precios { get; set; }
        //public DbSet<Usuario> Usuarios { get; set; }
    }
}