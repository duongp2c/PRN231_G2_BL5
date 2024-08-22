using PRN231_API.Common;
using PRN231_API.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_API.Repository
{
    public interface ISubjectRepository
    {
        Task<List<SubjectDTO>> GetAllSubjectsAsync();
        Task<SubjectDTO> GetSubjectByIdAsync(int id);
        Task<CustomResponse> CreateSubjectAsync(CreateSubjectDTO createSubjectDTO);
        Task<CustomResponse> UpdateSubjectAsync(int id, CreateSubjectDTO updateSubjectDTO);
        Task<CustomResponse> DeleteSubjectAsync(int id);
    }
}
