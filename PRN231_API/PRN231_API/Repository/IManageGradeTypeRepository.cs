using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public interface IManageGradeTypeRepository
    {
        Task<GradeType?> GetGradeTypeByIdAsync(int gradeTypeId);
        Task<List<GradeTypeDTO>> GetGradeTypeSubjectsAsync(int gradeTypeId);
        Task<string> AddGradeTypeSubjectAsync(GradeTypeDTO gradeTypeSubject);
        Task AddGradeTypeAsync(CreateGradeTypeDTO createGradeType);
    }
}
