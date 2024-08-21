using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN231_API.DTO;
using PRN231_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN231_API.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly SchoolDBContext _context;
        private readonly IMapper _mapper;

        public TeacherRepository(SchoolDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddTeacherAsync(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return teacher.TeacherId;
        }

        public async Task<int> AddAccountAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account.AccountId;
        }



        public async Task<bool> DeleteTeacherAsync(int teacherId)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Subjects)
                .Include(t => t.Account) // Include the account to delete it
                .FirstOrDefaultAsync(t => t.TeacherId == teacherId);

            if (teacher == null)
                return false;

            // Remove teacher from subjects
            foreach (var subject in teacher.Subjects)
            {
                subject.TeacherId = null;
                _context.Subjects.Update(subject);
            }

            // Remove the associated account if it exists
            if (teacher.Account != null)
            {
                _context.Accounts.Remove(teacher.Account);
            }

            // Remove the teacher
            _context.Teachers.Remove(teacher);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<TeacherDTO?> GetTeacherByIdAsync(int teacherId)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Subjects) // Include related subjects
                .Include(t => t.Account) // Include related account info (for Email, IsActive)
                .FirstOrDefaultAsync(t => t.TeacherId == teacherId);

            if (teacher == null)
            {
                // Log the ID that was not found
                Console.WriteLine($"Teacher with ID {teacherId} not found.");
                return null; // Return null if teacher is not found
            }

            // Manually map the properties to TeacherDTO
            var teacherDTO = new TeacherDTO
            {
                TeacherId = teacher.TeacherId,
                AccountId = teacher.AccountId ?? 0, // Assuming nullable, provide default
                TeacherName = teacher.TeacherName,
                Email = teacher.Account?.Email ?? string.Empty, // Safely handle null Account
                IsActive = teacher.Account?.IsActive ?? false, // Safely handle null Account
                Subjects = teacher.Subjects.Select(s => new SubjectDTO
                {
                    SubjectId = s.SubjectId,
                    SubjectName = s.SubjectName
                }).ToList() // Map subjects
            };

            return teacherDTO;
        }


        public async Task<List<TeacherDTO>> SearchTeachersByNameAsync(string name)
        {
            var teachersQuery = _context.Teachers
                .Include(t => t.Account)
                .Include(t => t.Subjects)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                teachersQuery = teachersQuery.Where(t => t.TeacherName.Contains(name));
            }

            var teachers = await teachersQuery.ToListAsync();

            // Map to TeacherDTO manually
            return teachers.Select(t => new TeacherDTO
            {
                TeacherId = t.TeacherId,
                AccountId = t.AccountId ?? 0,
                TeacherName = t.TeacherName,
                Email = t.Account?.Email ?? string.Empty,
                Subjects = t.Subjects.Select(s => new SubjectDTO
                {
                    SubjectId = s.SubjectId,
                    SubjectName = s.SubjectName
                }).ToList(),
                IsActive = t.Account?.IsActive ?? false
            }).ToList();
        }




        public async Task<List<TeacherDTO>> GetAllTeachersAsync()
        {
            var teachers = await _context.Teachers
                .Include(t => t.Account)
                .Include(t => t.Subjects)
                .Select(t => new TeacherDTO
                {
                    TeacherId = t.TeacherId,
                    AccountId = (int)t.AccountId,
                    TeacherName = t.TeacherName,
                    Email = t.Account.Email,
                    IsActive = t.Account.IsActive,
                    Subjects = t.Subjects.Select(s => new SubjectDTO
                    {
                        SubjectId = s.SubjectId,
                        SubjectName = s.SubjectName
                    }).ToList()
                })
                .ToListAsync();

            return teachers;
        }





        public async Task<TeacherDTO> CreateTeacherAsync(CreateTeacherDTO createTeacherDTO)
        {
            // Create Account manually
            var account = new Account
            {
                Email = createTeacherDTO.Email,
                Password = createTeacherDTO.Password,
                IsActive = createTeacherDTO.IsActive,
                Type = "Teacher"
            };
            await AddAccountAsync(account);

            // Create Teacher manually
            var teacher = new Teacher
            {
                TeacherName = createTeacherDTO.TeacherName,
                AccountId = account.AccountId
            };
            await AddTeacherAsync(teacher);

            // Manually map Teacher and Account to TeacherDTO
            var teacherDTO = new TeacherDTO
            {
                TeacherId = teacher.TeacherId,
                AccountId = account.AccountId,
                TeacherName = teacher.TeacherName,
                Email = account.Email,
                IsActive = account.IsActive,
                Subjects = new List<SubjectDTO>() // Empty list as no subjects are added
            };

            return teacherDTO;
        }



        public async Task<bool> EditActiveTeacherAsync(int teacherId, bool isActive)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.TeacherId == teacherId);

            if (teacher == null || teacher.Account == null)
            {
                return false; // Teacher or account does not exist
            }

            // Update the IsActive status
            teacher.Account.IsActive = isActive;

            // Save changes to the database
            await _context.SaveChangesAsync(); // Ensure the changes are saved

            return true;
        }



    }
}
