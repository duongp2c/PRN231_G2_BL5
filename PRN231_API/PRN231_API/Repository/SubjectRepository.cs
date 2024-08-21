using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN231_API.DTO;
using PRN231_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN231_API.Repository
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly SchoolDBContext _context;
        private readonly IMapper _mapper;

        public SubjectRepository(SchoolDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SubjectDTO>> GetAllSubjectsAsync()
        {
            var subjects = await _context.Subjects
                .Include(s => s.Evaluations) // Include related evaluations if needed
                .Include(s => s.StudentSubjects) // Include related student subjects if needed
                .ToListAsync();
            return _mapper.Map<List<SubjectDTO>>(subjects);
        }

        public async Task<SubjectDTO> GetSubjectByIdAsync(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Evaluations) // Include related evaluations if needed
                .Include(s => s.StudentSubjects) // Include related student subjects if needed
                .FirstOrDefaultAsync(s => s.SubjectId == id);
            return _mapper.Map<SubjectDTO>(subject);
        }

        public async Task CreateSubjectAsync(CreateSubjectDTO createSubjectDTO)
        {
            try
            {
                var subject = _mapper.Map<Subject>(createSubjectDTO);

                // Set the teachers if provided
                if (createSubjectDTO.TeacherIds != null && createSubjectDTO.TeacherIds.Any())
                {
                    var teachers = await _context.Teachers
                        .Where(t => createSubjectDTO.TeacherIds.Contains(t.TeacherId))
                        .ToListAsync();
                    subject.Teacher = teachers.FirstOrDefault(); // Adjust as needed
                }

                _context.Subjects.Add(subject);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                throw; // Rethrow the exception to be caught in the controller
            }
        }



        public async Task UpdateSubjectAsync(int id, CreateSubjectDTO updateSubjectDTO)
        {
            var existingSubject = await _context.Subjects
                .Include(s => s.Evaluations)
                .Include(s => s.StudentSubjects)
                .FirstOrDefaultAsync(s => s.SubjectId == id);

            if (existingSubject == null)
                return;

            // Update subject details
            _mapper.Map(updateSubjectDTO, existingSubject);

            // Update the teacher if provided
            if (updateSubjectDTO.TeacherIds.Any())
            {
                var teacher = await _context.Teachers.FindAsync(updateSubjectDTO.TeacherIds.First());
                existingSubject.Teacher = teacher;
            }
            else
            {
                existingSubject.Teacher = null; // Clear teacher if not provided
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteSubjectAsync(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Evaluations) // Check if there are any evaluations associated
                .Include(s => s.StudentSubjects) // Check if there are any student subjects associated
                .FirstOrDefaultAsync(s => s.SubjectId == id);

            if (subject == null)
                return 0; // Subject not found

            if (subject.Evaluations.Any() || subject.StudentSubjects.Any())
                return -1; // Cannot delete if associated evaluations or student subjects exist

            // Remove subject
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return 1; // Successfully deleted
        }
    }
}
