using Microsoft.EntityFrameworkCore;
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
            List<StudentSubject> list = await _context.StudentSubjects
                .Where(ss => ss.StudentId == studentId).Include("Subject")
                .ToListAsync();
            
            return list;
        }

        public async Task AddStudentSubjectAsync(StudentSubject studentSubject)
        {
            _context.StudentSubjects.Add(studentSubject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateStudentDetailAsync(StudentDetail student)
        {
            _context.StudentDetails.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task<StudentDetail?> GetStudentDetailByIdAsync(int studentId)
        {
            return await _context.StudentDetails
                .Where(s => s.StudentId == studentId)
                .FirstOrDefaultAsync();
        }
    }
}
