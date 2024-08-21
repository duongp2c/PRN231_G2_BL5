using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN231_API.DTO;
using PRN231_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN231_API.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolDBContext _context;
        private readonly IMapper _mapper;

        public StudentRepository(SchoolDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<List<StudentDTO>> GetAllStudentAsync()
        {
            var students = await _context.Students
                .Include(s => s.StudentDetail)
                .Include(s => s.StudentSubjects)
                    .ThenInclude(ss => ss.Subject)
                .Include(s => s.Account) // Include the Account entity to get IsActive status
                .ToListAsync();

            return _mapper.Map<List<StudentDTO>>(students);
        }

        public async Task<string> DeleteStudentAsync(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.StudentDetail)
                .Include(s => s.StudentSubjects)
                    .ThenInclude(ss => ss.Subject)
                .Include(s => s.Account) // Include the account to check its status
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
                return "Student not found"; // Student does not exist

            // Check if the associated account is active
            if (student.Account == null || student.Account.IsActive)
                return "Cannot delete. Student is active."; // Cannot delete if account is active or does not exist

            // Remove related entities if they exist
            if (student.StudentSubjects != null && student.StudentSubjects.Any())
            {
                _context.StudentSubjects.RemoveRange(student.StudentSubjects);
            }

            if (student.StudentDetail != null)
            {
                _context.StudentDetails.Remove(student.StudentDetail);
            }

            if (student.Account != null)
            {
                _context.Accounts.Remove(student.Account);
            }

            _context.Students.Remove(student);

            // Save changes and return result
            await _context.SaveChangesAsync();
            return "Student successfully deleted.";
        }



        public async Task<bool> EditActiveStudentAsync(int studentId, bool isActive)
        {
            var student = await _context.Students
                .Include(s => s.Account)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null || student.Account == null)
            {
                return false; // Student or account does not exist
            }

            // Update the IsActive status
            student.Account.IsActive = isActive;

            // Save changes to the database
            await _context.SaveChangesAsync(); // Ensure the changes are saved

            return true;
        }
        public async Task<List<StudentDTO>> SearchStudentsByNameAsync(string name)
        {
            var studentsQuery = _context.Students
                .Include(s => s.StudentDetail)
                .Include(s => s.StudentSubjects)
                    .ThenInclude(ss => ss.Subject)
                .Include(s => s.Account)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                studentsQuery = studentsQuery.Where(s => s.Name.Contains(name));
            }

            var students = await studentsQuery.ToListAsync();

            return _mapper.Map<List<StudentDTO>>(students);
        }


    }
}
