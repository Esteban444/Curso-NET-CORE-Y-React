using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace LeerData
{
    class Program
    {
        static void Main(string[] args)
        {
           using(var db = new AppVentaCursosContext())
           {
               //Relacion 1 a 1
               /*var cursos = db.Cursos.Include(p => p.PrecioPromocion);
               foreach (var curso in cursos){
                   Console.WriteLine(curso.Titulo + "----"+ curso.PrecioPromocion.PrecioActual);
               }*/

               //Relacion 1 a muchos
               /*var cursos = db.Cursos.Include(c => c.ComentarioLista);
               foreach(var curso in cursos){
                   Console.WriteLine(curso.Titulo);
                   foreach (var comentario in curso.ComentarioLista){
                     Console.WriteLine("********" + comentario.ComentarioTexto)  ;
                   }
               }*/

               //Relacion de muchos a muchos

               var cursos = db.Cursos.Include(c => c.InstructorLink).ThenInclude(c => c.Instructor);
               foreach (var curso in cursos){
                   Console.WriteLine(curso.Titulo);
                   foreach (var inslink in curso.InstructorLink){
                       Console.WriteLine("********"  + inslink.Instructor.Nombre);
                   }
               }
           }
        }
    }
}
