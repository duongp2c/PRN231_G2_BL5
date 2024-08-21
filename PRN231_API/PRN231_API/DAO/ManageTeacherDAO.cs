using PRN231_API.DTO;
using PRN231_API.Models;
using PRN231_API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_API.DAO
{
    public class ManageTeacherDAO
    {
        private readonly ITeacherRepository _teacherRepository;

        public ManageTeacherDAO(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<TeacherDTO?> GetTeacherByIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetTeacherByIdAsync(teacherId);
            if (teacher == null)
                return null; // Trả về null nếu không tìm thấy giáo viên

            return teacher;
        }


        public async Task<List<TeacherDTO>> GetAllTeachersWithSubjectAsync()
        {
            return await _teacherRepository.GetAllTeachersAsync();
        }

        public async Task<TeacherDTO> CreateTeacherAsync(CreateTeacherDTO createTeacherDTO)
        {
            return await _teacherRepository.CreateTeacherAsync(createTeacherDTO);
        }

        public async Task<bool> EditActiveTeacherAsync(int teacherId, bool isActive)
        {
            return await _teacherRepository.EditActiveTeacherAsync(teacherId, isActive);
        }


        public async Task<bool> DeleteTeacherAsync(int teacherId)
        {
            return await _teacherRepository.DeleteTeacherAsync(teacherId);
        }
        public async Task<List<TeacherDTO>> SearchTeachersByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await GetAllTeachersWithSubjectAsync();
            }

            var teachers = await _teacherRepository.SearchTeachersByNameAsync(name);
            return teachers;
        }

    }
}
