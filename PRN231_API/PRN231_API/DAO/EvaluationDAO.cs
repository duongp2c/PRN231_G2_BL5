using PRN231_API.Models;
using PRN231_API.Repository;
using PRN231_API.DTO;

namespace PRN231_API.DAO
{

    public class EvaluationDAO
    {
        private readonly IEvaluationRepository _eRepository;
        private readonly IStudentRepository _sRepository;

        public EvaluationDAO(IEvaluationRepository eRepository, IStudentRepository sRepository)
        {
            _eRepository = eRepository;
            _sRepository = sRepository;
        }
        public async Task<List<GradeDTO>> GetStudentSubjectGradeAsync(int accountId, int subjectId)
        {
            var student = await _sRepository.GetStudentByIdAsync(accountId);
            List<Evaluation> evaluations = await _eRepository.GetEvaluationBySubjectAndStudent(student.StudentId, subjectId);
            List<GradeDTO> grades = new List<GradeDTO>();   
            foreach (Evaluation e in evaluations)
            {
                var grade = new GradeDTO
                {
                    Name = e.GradeType.GradeTypeName,
                    Grade = e.Grade,
                    AdditionalInfo = e.AdditionExplanation
                };
                grades.Add(grade);
            }
            return grades;
        }
    }
}
