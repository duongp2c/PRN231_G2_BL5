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
            return _context.Subjects.AsEnumerable(); 
        }

        public IEnumerable<SubjectIsAndNameDTO> GetAllSubjectsIDandName()
        {
            var result = _context.Subjects
                     .Include(s => s.Teacher)
                     .Select(s => new SubjectIsAndNameDTO
                     {
                         SubjectId = s.SubjectId,
                         SubjectName = s.SubjectName
                     }).ToList();
            return result;
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
        public async Task<IEnumerable<object>> GetSubjectsByStudentIDAsync(int accountId)
        {
            // Chờ kết quả của GetStudentIdByAccountIdAsync
            var studentId = await GetStudentIdByAccountIdAsync(accountId);

            // Nếu không tìm thấy studentId, trả về danh sách rỗng
            if (studentId == null)
            {
                return new List<object>();
            }

            // Truy vấn để lấy danh sách môn học của sinh viên
            var subjects = await _context.StudentSubjects
                .Include(ss => ss.Subject)
                .ThenInclude(s => s.Teacher)
                .Where(ss => ss.StudentId == studentId)
                .Select(ss => new
                {
                    SubjectName = ss.Subject.SubjectName,
                    TeacherName = ss.Subject.Teacher.TeacherName,
                    IsComplete = ss.IsComplete ? "Đã hoàn thành" : "Đang học"
                })
                .ToListAsync();

            return subjects;
        }


    }
}
