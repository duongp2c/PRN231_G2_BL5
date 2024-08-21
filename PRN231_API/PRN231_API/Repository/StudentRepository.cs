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

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            //return await _context.Students.FindAsync(studentId);
            return await _context.Students.Where(s => s.AccountId == id).FirstOrDefaultAsync();
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

        public async Task<StudentDetail?> GetStudentDetailByIdAsync(int id)
        {
            return await _context.StudentDetails
                .Where(s => s.StudentId == id)
                .FirstOrDefaultAsync();
        }
    }
}
