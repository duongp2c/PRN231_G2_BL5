using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN231_API.Common;
using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.Repository;

public class ManageGradeTypeDAO
{
    private readonly IManageGradeTypeRepository _gradeTypeRepository;
    private readonly IMapper _mapper;

    public ManageGradeTypeDAO(IManageGradeTypeRepository gradeTypeRepository, IMapper mapper)
    {
        _gradeTypeRepository = gradeTypeRepository;
        _mapper = mapper;
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
        };
    }

    public async Task<List<GradeTypeDTO>> GetAllGradeTypesAsync()
    {
        return await _gradeTypeRepository.GetAllGradeTypesAsync();

    }

    public async Task<CustomResponse> CreateGradeTypeAsync(CreateGradeTypeDTO createGradeTypeDTO)
    {
        return await _gradeTypeRepository.CreateGradeTypeAsync(createGradeTypeDTO);
    }

    public async Task<CustomResponse> DeleteGradeTypeAsync(int id)
    {
        return await _gradeTypeRepository.DeleteGradeTypeAsync(id);
    }

}

