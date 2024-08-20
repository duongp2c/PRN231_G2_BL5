using PRN231_API.DAO;
using PRN231_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IStudentDAO _studentDAO;

        public StudentRepository(IStudentDAO studentDAO)
        {
            _studentDAO = studentDAO;
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _studentDAO.GetStudentByIdAsync(studentId);
        }

        public async Task<IEnumerable<Evaluation>> GetEvaluationsByStudentIdAsync(int studentId)
        {
            return await _studentDAO.GetEvaluationsByStudentIdAsync(studentId);
        }

        public async Task UpdateEvaluationAsync(Evaluation evaluation)
        {
            await _studentDAO.UpdateEvaluationAsync(evaluation);
        }
    }
}
