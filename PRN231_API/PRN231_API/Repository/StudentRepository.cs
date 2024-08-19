using Microsoft.EntityFrameworkCore;
using PRN231_API.DAO;
using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public class StudentRepository : IStudentRepository
    {
        
        private readonly SchoolDBContext _context;
        

        public StudentRepository(SchoolDBContext context)
        {
            _context = context;
           
        }

        public async Task<Student?> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students.FindAsync(studentId);
        }

        public async Task<List<StudentSubject>> GetStudentSubjectsAsync(int studentId)
        {
            return await _context.StudentSubjects
                .Where(ss => ss.StudentId == studentId)
                .ToListAsync();
        }

        public async Task AddStudentSubjectAsync(StudentSubject studentSubject)
        {
            _context.StudentSubjects.Add(studentSubject);
            await _context.SaveChangesAsync();
        }

    }
}
