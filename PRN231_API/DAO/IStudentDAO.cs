using PRN231_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_API.DAO
{
    public interface IStudentDAO
    {
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<IEnumerable<Evaluation>> GetEvaluationsByStudentIdAsync(int studentId);
        Task UpdateEvaluationAsync(Evaluation evaluation);
    }
}
