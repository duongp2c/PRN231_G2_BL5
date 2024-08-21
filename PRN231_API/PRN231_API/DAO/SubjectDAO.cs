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
        public async Task<IEnumerable<object>> GetSubjectsByStudentIDAsync(int accountId)
        {
            // Gọi phương thức bất đồng bộ để lấy danh sách môn học
            var obj = await _subjectRepository.GetSubjectsByStudentIDAsync(accountId);

            // Nếu không tìm thấy dữ liệu, trả về danh sách rỗng
            if (obj == null)
            {
                return new List<object>();
            }

            // Trả về danh sách môn học
            return obj;
        }
        public IEnumerable<SubjectDTO> GetAllSubjects2Field()
        {
            var result = _subjectRepository.GetAllSubjects2Field();
            return result;
        }

        public IEnumerable<SubjectIsAndNameDTO> GetAllSubjectsIdAndName()
        {
            // Gọi phương thức GetAllSubjects từ ISubjectRepository
            var subjects = _subjectRepository.GetAllSubjectsIDandName();

            if (subjects == null)
            {
                return new List<SubjectIsAndNameDTO>();
            }
            return subjects;
        }
    }
}
