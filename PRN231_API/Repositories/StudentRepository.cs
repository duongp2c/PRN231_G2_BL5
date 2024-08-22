using PRN231_API.DAO;
using PRN231_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

using PRN231_API.DTO;
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

        public async Task UpdateEvaluationAsync(EvaluationDTO evaluationDTO)
        {
            await _studentDAO.UpdateEvaluationAsync(evaluationDTO);
        }

        public async Task<IEnumerable<StudentDTO>> GetStudentsBySubjectAsync(int subjectId)
        {
            return await _studentDAO.GetStudentsBySubjectAsync(subjectId);
        }

    }
}
