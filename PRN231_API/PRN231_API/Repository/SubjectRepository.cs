using Microsoft.EntityFrameworkCore;
using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly SchoolDBContext _context;

        public SubjectRepository(SchoolDBContext context)
        {
            _context = context;
        }
       

        public IEnumerable<Subject> GetAllSubjects()
        {
            return _context.Subjects.
                
                AsEnumerable(); 
        }

        public IEnumerable<SubjectDTO> GetAllSubjects2Field()
        {
            var result = _context.Subjects
                     .Include(s => s.Teacher) 
                     .Select(s => new SubjectDTO
                     {
                         SubjectId = s.SubjectId,
                         SubjectName = s.SubjectName,
                         TeacherId = s.TeacherId,
                         TeacherName = s.Teacher.TeacherName
                     }).ToList();
            return result;
        }

        public IEnumerable<object> GetSubjectsByStudentID(int studentId)
        {
            var rs = _context.StudentSubjects
                             .Include(x => x.Subject)
                             .ThenInclude(y => y.Teacher)
                             .Where(ss => ss.StudentId == studentId)
                             .Select(ss => new
                             {
                                 SubjectName = ss.Subject.SubjectName,
                                 TeacherName = ss.Subject.Teacher.TeacherName,
                                 IsComplete = ss.IsComplete == true ? "Đã hoàn thành" : "Đang học"
                             })
                             .ToList();

            return rs;
        }

        
    }
}
