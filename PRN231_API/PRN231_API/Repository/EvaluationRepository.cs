using Microsoft.EntityFrameworkCore;
using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public class EvaluationRepository : IEvaluationRepository
    {
        private readonly SchoolDBContext _context;


        public EvaluationRepository(SchoolDBContext context)
        {
            _context = context;

        }
        public async Task<Evaluation?> GetEvaluationByID(int evaluationId)
        {
            return await _context.Evaluations.FindAsync(evaluationId);
        }

        public async Task<List<Evaluation>> GetEvaluationByStudentID(int studentId)
        {
            return await _context.Evaluations
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<Evaluation>> GetEvaluationBySubjectAndStudent(int subjectId, int studentId)
        {
            return await _context.Evaluations
                .Where(e => e.SubjectId == subjectId && e.StudentId == studentId).Include("GradeType")
                .ToListAsync();
        }

        public async Task<List<Evaluation>> GetEvaluationBySubjectID(int subjectId)
        {
            return await _context.Evaluations
                .Where(e => e.SubjectId == subjectId)
                .ToListAsync();
        }

        public IEnumerable<Evaluation> GetEvaluations()
        {
            throw new NotImplementedException();
        }

        public async Task<GradeType?> GetGradeTypesByID(int gradeTypeId)
        {
            return await _context.GradeTypes.FindAsync(gradeTypeId);
        }
    }
}
