// DAO/IEvaluationDAO.cs
using PRN231_API.DTO;
using PRN231_API.Models;

public interface IEvaluationDAO
{
    Task<IEnumerable<EvaluationDTO>> GetEvaluationsByStudentIdAsync(int studentId);
    Task<Evaluation> GetEvaluationByIdAsync(int evaluationId);
    Task<IEnumerable<Evaluation>> GetAllEvaluationsAsync();
    Task<Evaluation> AddEvaluationAsync(Evaluation evaluation);
    Task UpdateEvaluationAsync(EvaluationDTO evaluationDTO);
    Task DeleteEvaluationAsync(int evaluationId);
}
