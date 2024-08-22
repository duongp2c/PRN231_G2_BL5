
using PRN231_API.Models;
using PRN231_API.DTO;

namespace PRN231_API.Repositories
{
    public class EvaluationRepository : IEvaluationRepository
    {
        private readonly IEvaluationDAO _dao;

        public EvaluationRepository(IEvaluationDAO dao)
        {
            _dao = dao;
        }

        public async Task<Evaluation> GetEvaluationByIdAsync(int evaluationId)
        {
            return await _dao.GetEvaluationByIdAsync(evaluationId);
        }

        public async Task<IEnumerable<Evaluation>> GetAllEvaluationsAsync()
        {
            return await _dao.GetAllEvaluationsAsync();
        }

        public async Task<Evaluation> AddEvaluationAsync(Evaluation evaluation)
        {
            return await _dao.AddEvaluationAsync(evaluation);
        }

        public async Task UpdateEvaluationAsync(EvaluationDTO evaluationDTO)
        {
            await _dao.UpdateEvaluationAsync(evaluationDTO);
        }

        public async Task DeleteEvaluationAsync(int evaluationId)
        {
            await _dao.DeleteEvaluationAsync(evaluationId);
        }
    }
}
