using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public interface ISubjectRepository
    {
        IEnumerable<Subject> GetAllSubjects();
        Task<IEnumerable<object>> GetSubjectsByStudentIDAsync(int accountId);
        Task<int?> GetStudentIdByAccountIdAsync(int accountId);
        IEnumerable<SubjectDTO> GetAllSubjects2Field();
        IEnumerable<SubjectIsAndNameDTO> GetAllSubjectsIDandName();
    }
}
