using PRN231_API.Common;
using PRN231_API.DTO;
using PRN231_API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_API.DAO
{
    public class ManageSubjectDAO
    {
        private readonly ISubjectRepository _subjectRepository;

        public ManageSubjectDAO(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<List<SubjectDTO>> GetAllSubjectsAsync()
        {
            return await _subjectRepository.GetAllSubjectsAsync();
        }

        public async Task<SubjectDTO?> GetSubjectByIdAsync(int subjectId)
        {
            return await _subjectRepository.GetSubjectByIdAsync(subjectId);
        }

        public async Task<CustomResponse> CreateSubjectAsync(CreateSubjectDTO createSubjectDTO)
        {
            return await _subjectRepository.CreateSubjectAsync(createSubjectDTO);

        }

        public async Task<CustomResponse?> UpdateSubjectAsync(int subjectId, CreateSubjectDTO updateSubjectDTO)
        {
            return await _subjectRepository.UpdateSubjectAsync(subjectId, updateSubjectDTO);
        }

        public async Task<CustomResponse> DeleteSubjectAsync(int subjectId)
        {
            return await _subjectRepository.DeleteSubjectAsync(subjectId);
        }
    }
}
