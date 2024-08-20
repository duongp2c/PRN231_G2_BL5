using PRN231_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_API.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<IEnumerable<Evaluation>> GetEvaluationsByStudentIdAsync(int studentId);
        Task UpdateEvaluationAsync(Evaluation evaluation);
    }
}
