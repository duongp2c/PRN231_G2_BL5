using PRN231_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_API.Services
{
    public interface IStudentService
    {
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<IEnumerable<Evaluation>> GetEvaluationsByStudentIdAsync(int studentId);
        Task UpdateEvaluationAsync(Evaluation evaluation);
    }
}
