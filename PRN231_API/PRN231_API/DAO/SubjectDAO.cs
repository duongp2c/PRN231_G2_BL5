using Microsoft.EntityFrameworkCore;
using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.Repository;

namespace PRN231_API.DAO
{
    public class SubjectDAO
    {
        private readonly ISubjectRepository _subjectRepository;


        public SubjectDAO(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;

        }
        public IEnumerable<Subject> GetAllSubjects()
        {
            // Gọi phương thức GetAllSubjects từ ISubjectRepository
            var subjects = _subjectRepository.GetAllSubjects();

            if(subjects == null)
            {
                return new List<Subject>();
            }
            return subjects;
        }
        public IEnumerable<object> GetSubjectsByStudentID(int studentId)
        {
            var obj = _subjectRepository.GetSubjectsByStudentID(studentId);
            if(obj == null)
            {
                return new List<object>();
            }
            return obj;

        }
        public IEnumerable<SubjectDTO> GetAllSubjects2Field()
        {
            var result = _subjectRepository.GetAllSubjects2Field();
            return result;
        }

    }
}
