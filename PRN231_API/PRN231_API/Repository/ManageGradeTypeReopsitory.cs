using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN231_API.Common;
using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public class ManageGradeTypeRepository : IManageGradeTypeRepository
    {
        private readonly SchoolDBContext _context;
        private readonly IMapper _mapper;


        public ManageGradeTypeRepository(SchoolDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GradeType> GetGradeTypeByIdAsync(int gradeTypeId)
        {
            return await _context.GradeTypes
                .FirstOrDefaultAsync(gt => gt.GradeTypeId == gradeTypeId);
        }

        public async Task<List<GradeTypeDTO>> GetAllGradeTypesAsync()
        {
            try
            {
                var subjects = await _context.GradeTypes
                    .Select(t => new GradeTypeDTO
                    {
                        GradeTypeId = t.GradeTypeId,
                        GradeTypeName = t.GradeTypeName
                    })
                    .ToListAsync();

                return subjects;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<CustomResponse> CreateGradeTypeAsync(CreateGradeTypeDTO createGradeType)
        {

            if (createGradeType == null)
            {
                return new CustomResponse { Message = "Cannot get grade value", StatusCode = 400 };
            }

            var gradeType = new GradeType
            {
                GradeTypeId = createGradeType.GradeTypeID,
                GradeTypeName = createGradeType.GradeTypeName,
            };

            _context.GradeTypes.Add(gradeType);
            await _context.SaveChangesAsync();
            return new CustomResponse { Message = "Create Grade Success", StatusCode = 200 };
        }

        public async Task<CustomResponse> DeleteGradeTypeAsync(int id)
        {
            var grade = await _context.GradeTypes.FirstOrDefaultAsync(s => s.GradeTypeId == id);

            if (grade == null)
                return new CustomResponse { Message = "Cannot find this grade", StatusCode = 400 };

            _context.GradeTypes.Remove(grade);
            await _context.SaveChangesAsync();

            return new CustomResponse { Message = "Delete Grade Type Success", StatusCode = 200 };
        }
    }
}
