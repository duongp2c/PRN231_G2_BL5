using PRN231_API.Models;
using PRN231_API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_API.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _studentRepository.GetStudentByIdAsync(studentId);
        }

        public async Task<IEnumerable<Evaluation>> GetEvaluationsByStudentIdAsync(int studentId)
        {
            return await _studentRepository.GetEvaluationsByStudentIdAsync(studentId);
        }

        public async Task UpdateEvaluationAsync(Evaluation evaluation)
        {
            // Add any additional business logic here if needed
            await _studentRepository.UpdateEvaluationAsync(evaluation);
        }
    }
}
