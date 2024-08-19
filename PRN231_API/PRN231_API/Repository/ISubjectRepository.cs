using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public interface ISubjectRepository
    {
        IEnumerable<Subject> GetAllSubjects();
        IEnumerable<object> GetSubjectsByStudentID(int studentId);
        IEnumerable<SubjectDTO> GetAllSubjects2Field();
    }
}
