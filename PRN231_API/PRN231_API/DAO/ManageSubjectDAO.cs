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

        public async Task<SubjectDTO> CreateSubjectAsync(CreateSubjectDTO createSubjectDTO)
        {
            await _subjectRepository.CreateSubjectAsync(createSubjectDTO);
            return await _subjectRepository.GetSubjectByIdAsync(createSubjectDTO.SubjectId);
        }

        public async Task<SubjectDTO?> UpdateSubjectAsync(int subjectId, CreateSubjectDTO updateSubjectDTO)
        {
            await _subjectRepository.UpdateSubjectAsync(subjectId, updateSubjectDTO);
            return await _subjectRepository.GetSubjectByIdAsync(subjectId);
        }

        public async Task<bool> DeleteSubjectAsync(int subjectId)
        {
            return await _subjectRepository.DeleteSubjectAsync(subjectId) == 1;
        }
    }
}
