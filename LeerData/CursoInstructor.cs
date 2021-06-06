using System.Collections.Generic;

 namespace LeerData
{
     public class CursoInstructor
    {
        public int InstructorId { get; set; }
        public int CursoId { get; set; }

        public Curso Cursos { get; set; }
        public Instructor Instructor { get; set; }
    }
}