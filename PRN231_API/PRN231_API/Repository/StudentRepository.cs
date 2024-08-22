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
        public async Task<int?> GetStudentIdByAccountIdAsync(int accountId)
        {

            // Retrieve the student while considering the possibility of null values
            var student = await _context.Students
                                        .Where(x => x.AccountId == accountId)
                                        .FirstOrDefaultAsync();

            // Check if student is null
            if (student == null)
            {
                return null;  // Return null if the student was not found
            }

            // If found, return the StudentId
            return student.StudentId;  // StudentId is nullable, so this is safe
        }

        public Task<Student?> GetStudentByAccountIdAsync(int studentId)
        {
            throw new NotImplementedException();
        }

        public Task<StudentDetail?> GetStudentDetailByIdAsync(int studentId)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentSubject>> GetStudentSubjectsByStudentIdAsync(int studentId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStudentAsync(Student student)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStudentDetailAsync(StudentDetail student)
        {
            throw new NotImplementedException();
        }

        public Task AddStudentSubjectAsync(StudentSubject studentSubject)
        {
            throw new NotImplementedException();
        }
    }
}
