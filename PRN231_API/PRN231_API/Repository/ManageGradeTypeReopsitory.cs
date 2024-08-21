/*using Microsoft.EntityFrameworkCore;
using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API.Repository
{
    public class ManageGradeTypeRepository : IManageGradeTypeRepository
    {
        private readonly SchoolDBContext _context;

        public ManageGradeTypeRepository(SchoolDBContext context)
        {
            _context = context;
        }

        public async Task<GradeType?> GetGradeTypeByIdAsync(int gradeTypeId)
        {
            return await _context.GradeTypes
                .Include(gt => gt.Evaluations)
                    .ThenInclude(e => e.Subject)
                .FirstOrDefaultAsync(gt => gt.GradeTypeId == gradeTypeId);
        }

        public async Task<List<GradeTypeDTO>> GetGradeTypeSubjectsAsync(int gradeTypeId)
        {
            return await _context.Evaluations
                .Where(e => e.GradeTypeId == gradeTypeId)
                .Select(e => new GradeTypeDTO
                {
                    GreadeTypeId = e.GradeTypeId.GetValueOrDefault(),
                    SubjectId = e.SubjectId.GetValueOrDefault(),
                    GradeType = e.GradeType,
                    Subject = e.Subject
                })
                .ToListAsync();
        }

        public async Task<string> AddGradeTypeSubjectAsync(GradeTypeDTO gradeTypeSubjectDTO)
        {
            if (gradeTypeSubjectDTO == null)
                throw new ArgumentNullException(nameof(gradeTypeSubjectDTO));

            // Fetch the GradeType and Subject
            var gradeType = await _context.GradeTypes.FindAsync(gradeTypeSubjectDTO.GreadeTypeId);
            var subject = await _context.Subjects.FindAsync(gradeTypeSubjectDTO.SubjectId);

            if (gradeType == null)
                return "GradeType not found.";

            if (subject == null)
                return "Subject not found.";

            // Check if the GradeType and Subject are already associated
            var existingAssociation = await _context.Evaluations
                .AnyAsync(e => e.GradeTypeId == gradeTypeSubjectDTO.GreadeTypeId && e.SubjectId == gradeTypeSubjectDTO.SubjectId);

            if (existingAssociation)
                return "GradeType is already associated with this Subject.";

            // Create new GradeTypeSubject association
            var gradeTypeSubject = new Evaluation
            {
                GradeTypeId = gradeTypeSubjectDTO.GreadeTypeId,
                SubjectId = gradeTypeSubjectDTO.SubjectId,
                Grade = 0, // Default value, adjust as needed
                AdditionExplanation = string.Empty // Default value, adjust as needed
            };

            _context.Evaluations.Add(gradeTypeSubject);
            await _context.SaveChangesAsync();

            return "GradeType successfully associated with Subject.";
        }


        public async Task AddGradeTypeAsync(CreateGradeTypeDTO createGradeType)
        {
            if (createGradeType == null)
                throw new ArgumentNullException(nameof(createGradeType));

            var gradeType = new GradeType
            {
                GradeTypeName = createGradeType.GradeTypeName,
                GradeTypeWeight = createGradeType.GradeTypeWeight
            };

            _context.GradeTypes.Add(gradeType);
            await _context.SaveChangesAsync();
        }
    }
}
*/