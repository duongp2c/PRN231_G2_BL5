using PRN231_API.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_API.Repository
{
    public interface ISubjectRepository
    {
        Task<List<SubjectDTO>> GetAllSubjectsAsync();
        Task<SubjectDTO> GetSubjectByIdAsync(int id);
        Task CreateSubjectAsync(CreateSubjectDTO createSubjectDTO);
        Task UpdateSubjectAsync(int id, CreateSubjectDTO updateSubjectDTO);
        Task<int> DeleteSubjectAsync(int id);
    }
}
