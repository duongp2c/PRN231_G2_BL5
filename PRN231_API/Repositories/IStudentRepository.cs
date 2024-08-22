using PRN231_API.Models;
using PRN231_API.DTO;

namespace PRN231_API.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<IEnumerable<Evaluation>> GetEvaluationsByStudentIdAsync(int studentId);
        Task UpdateEvaluationAsync(EvaluationDTO evaluationDTO);
        Task<IEnumerable<StudentDTO>> GetStudentsBySubjectAsync(int subjectId);
    }
}
