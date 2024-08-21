using PRN231_API.DTO;
using PRN231_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_API.Repository
{
    public interface ITeacherRepository
    {
        Task<int> AddTeacherAsync(Teacher teacher);
        Task<int> AddAccountAsync(Account account);
        Task<bool> EditActiveTeacherAsync(int teacherId, bool isActive);
       
        Task<string> DeleteTeacherAsync(int teacherId);
        Task<TeacherDTO?> GetTeacherByIdAsync(int teacherId);
        Task<List<TeacherDTO>> GetAllTeachersAsync();
        Task<TeacherDTO> CreateTeacherAsync(CreateTeacherDTO createTeacherDTO);
        Task<List<TeacherDTO>> SearchTeachersByNameAsync(string name);
    }
}
