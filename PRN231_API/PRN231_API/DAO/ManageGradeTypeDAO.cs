/*using PRN231_API.DTO;
using PRN231_API.Repository;

public class ManageGradeTypeDAO
{
    private readonly IManageGradeTypeRepository _gradeTypeRepository;

    public ManageGradeTypeDAO(IManageGradeTypeRepository gradeTypeRepository)
    {
        _gradeTypeRepository = gradeTypeRepository;
    }

    

    public async Task<GradeTypeDTO?> GetGradeTypeByIdAsync(int gradeTypeId)
    {
        var gradeType = await _gradeTypeRepository.GetGradeTypeByIdAsync(gradeTypeId);
        if (gradeType == null)
            return null;

        return new GradeTypeDTO
        {
            GradeTypeId = gradeType.GradeTypeId,
            GradeTypeName = gradeType.GradeTypeName,
            GradeTypeWeight = (decimal)gradeType.GradeTypeWeight
        };
    }

    public async Task<GradeTypeDTO> CreateGradeTypeAsync(CreateGradeTypeDTO createGradeTypeDTO)
    {
        await _gradeTypeRepository.AddGradeTypeAsync(createGradeTypeDTO);
        var gradeType = await _gradeTypeRepository.GetGradeTypeByIdAsync(createGradeTypeDTO.GradeTypeID);
        return new GradeTypeDTO
        {
            GradeTypeId = gradeType.GradeTypeId,
            GradeTypeName = gradeType.GradeTypeName,
            GradeTypeWeight = (decimal)gradeType.GradeTypeWeight
        };
    }

    public async Task<string> AssociateGradeTypeWithSubjectAsync(GradeTypeDTO gradeTypeSubjectDTO)
    {
        return await _gradeTypeRepository.AddGradeTypeSubjectAsync(gradeTypeSubjectDTO);
    }
}

*/