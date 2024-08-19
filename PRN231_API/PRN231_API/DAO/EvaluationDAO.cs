using PRN231_API.Models;
using PRN231_API.Repository;
using PRN231_API.DTO;

namespace PRN231_API.DAO
{

    public class EvaluationDAO
    {
        private readonly IEvaluationRepository _eRepository;


        public EvaluationDAO(IEvaluationRepository eRepository)
        {
            _eRepository = eRepository;

        }
        public async Task<List<GradeDTO>> GetStudentSubjectGradeAsync(int studentId, int subjectId)
        {
            List<Evaluation> evaluations = await _eRepository.GetEvaluationBySubjectAndStudent(studentId, subjectId);
            List<GradeDTO> grades = new List<GradeDTO>();   
            foreach (Evaluation e in evaluations)
            {
                var grade = new GradeDTO
                {
                    Name = e.GradeType.GradeTypeName,
                    Weight = e.GradeType.GradeTypeWeight,
                    Grade = e.Grade,
                    AdditionalInfo = e.AdditionExplanation
                };
                grades.Add(grade);
            }
            return grades;
        }
    }
}
