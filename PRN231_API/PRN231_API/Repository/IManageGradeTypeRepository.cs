using PRN231_API.Common;
using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public interface IManageGradeTypeRepository
    {
        Task<GradeType> GetGradeTypeByIdAsync(int id);
        Task<List<GradeTypeDTO>> GetAllGradeTypesAsync();
        Task<CustomResponse> CreateGradeTypeAsync(CreateGradeTypeDTO createGradeType);
        Task<CustomResponse> DeleteGradeTypeAsync(int id);
    }
}
