using Microsoft.EntityFrameworkCore;
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
                .Include(e => e.Subject)
                .Include(e => e.GradeType)
                .ToListAsync();
        }

        public async Task UpdateEvaluationAsync(Evaluation evaluation)
        {
            _context.Evaluations.Update(evaluation);
            await _context.SaveChangesAsync();
        }
    }
}
