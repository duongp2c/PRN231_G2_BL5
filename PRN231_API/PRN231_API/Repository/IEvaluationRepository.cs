using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public interface IEvaluationRepository
    {
        IEnumerable<Evaluation> GetEvaluations();
        Task<Evaluation?> GetEvaluationByID(int evaluationId);
        Task<List<Evaluation>> GetEvaluationByStudentID(int studentId);
        Task<List<Evaluation>> GetEvaluationBySubjectID(int subjectId);
        Task<List<Evaluation>> GetEvaluationBySubjectAndStudent(int subjectId,int studentId);
        Task<GradeType?> GetGradeTypesByID(int gradeTypeId);

    }
}
