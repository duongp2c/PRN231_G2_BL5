using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_API.Common;
using PRN231_API.DTO;
using PRN231_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN231_API.Repository
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly SchoolDBContext _context;
        private readonly IMapper _mapper;

        public SubjectRepository(SchoolDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SubjectDTO>> GetAllSubjectsAsync()
        {
            var subjects = await _context.Subjects
                .Include(s => s.Teacher)
                .Select(t => new Subject
                {
                    SubjectId = t.SubjectId,
                    SubjectName = t.SubjectName,
                    Teacher = t.Teacher
                })
                .ToListAsync();

            return _mapper.Map<List<SubjectDTO>>(subjects);
        }

        public async Task<SubjectDTO> GetSubjectByIdAsync(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Evaluations) // Include related evaluations if needed
                .Include(s => s.StudentSubjects) // Include related student subjects if needed
                .FirstOrDefaultAsync(s => s.SubjectId == id);
            return _mapper.Map<SubjectDTO>(subject);
        }

        public async Task<CustomResponse> CreateSubjectAsync(CreateSubjectDTO createSubjectDTO)
        {
            try
            {
                var subject = _mapper.Map<Subject>(createSubjectDTO);

                if (createSubjectDTO.TeacherId == null)
                {
                    return new CustomResponse { Message = "Cannot get teacher value", StatusCode = 400 };
                }

                _context.Subjects.Add(subject);
                await _context.SaveChangesAsync();
                return new CustomResponse { Message =  "Create Subject Success", StatusCode = 200 };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<CustomResponse> UpdateSubjectAsync(int id, CreateSubjectDTO updateSubjectDTO)
        {


            var existingSubject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId == id);

            if (existingSubject == null)
                return new CustomResponse { Message = "Cannot get subject", StatusCode = 400 };
            var subject = _mapper.Map(updateSubjectDTO, existingSubject);

            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();

            return new CustomResponse { Message = "Update Subject Success", StatusCode = 200 };
        }

        public async Task<CustomResponse> DeleteSubjectAsync(int id)
        {
            bool checkSubjectExist = _context.StudentSubjects.Any(ss => ss.SubjectId == id);
            if (checkSubjectExist)
                return new CustomResponse { Message = "Cannot delete this subject because it been studied!", StatusCode = 400 };

            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId == id);

            if (subject == null)
                return new CustomResponse { Message = "Cannot find this subject", StatusCode = 401 };

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return new CustomResponse { Message = "Delete Subject Success", StatusCode = 200 };
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
