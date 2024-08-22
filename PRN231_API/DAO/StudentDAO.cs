using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_API.DTO;
using PRN231_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN231_API.DAO
{
    public class StudentDAO : IStudentDAO
    {
        private readonly SchoolDBContext _context;

        public StudentDAO(SchoolDBContext context)
        {
            _context = context;
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students
                .Include(s => s.StudentDetail)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task<IEnumerable<Evaluation>> GetEvaluationsByStudentIdAsync(int studentId)
        {
            return await _context.Evaluations
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Student)
                .Include(e => e.Subject)
                .Include(e => e.GradeType)
                .ToListAsync();
        }

        public async Task UpdateEvaluationAsync(EvaluationDTO evaluationDTO)
        {
            // Retrieve the existing evaluation entity
            var evaluation = await _context.Evaluations
                .FirstOrDefaultAsync(e => e.EvaluationId == evaluationDTO.EvaluationId);

            if (evaluation == null)
            {
                throw new KeyNotFoundException("Evaluation not found.");
            }

            // Update only the properties that you want to modify
            evaluation.Grade = evaluationDTO.Grade;
            evaluation.AdditionExplanation = evaluationDTO.AdditionExplanation;

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StudentDTO>> GetStudentsBySubjectAsync([FromQuery]int subjectId)
        {
            var students = await _context.StudentSubjects
                .Where(ss => ss.SubjectId == subjectId)
                .Include(ss => ss.Student)
                .Include(ss => ss.Subject)
                .ThenInclude(s => s.Teacher) // Ensure Teacher is included if needed
                .Select(ss => new StudentDTO
                {
                    StudentId = ss.Student.StudentId,
                    
                    Name = ss.Student.Name,
                    Age = ss.Student.Age,
                    IsRegularStudent = ss.Student.IsRegularStudent,
                    SubjectId = ss.Subject.SubjectId,
                    SubjectName = ss.Subject.SubjectName,
                    TeacherName = ss.Subject.Teacher != null ? ss.Subject.Teacher.TeacherName : "Unknown"
                })
                .ToListAsync();

            return students;
        }

    }
}
