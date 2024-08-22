using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Repositories
{
    // Repositories/IEvaluationRepository.cs
    public interface IEvaluationRepository
    {
        Task<Evaluation> GetEvaluationByIdAsync(int evaluationId);
        Task<IEnumerable<Evaluation>> GetAllEvaluationsAsync();
        Task<Evaluation> AddEvaluationAsync(Evaluation evaluation);
        Task UpdateEvaluationAsync(EvaluationDTO evaluationDTO);
        Task DeleteEvaluationAsync(int evaluationId);
    }

}
