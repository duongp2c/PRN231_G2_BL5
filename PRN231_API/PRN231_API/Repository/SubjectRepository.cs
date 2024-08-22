using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_API.Common;
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
                .Include(s => s.Teacher)
                .Select(t => new Subject
                {
                    SubjectId = t.SubjectId,
                    SubjectName = t.SubjectName,
                    Teacher = t.Teacher
                })
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

        public async Task<CustomResponse> CreateSubjectAsync(CreateSubjectDTO createSubjectDTO)
        {
            try
            {
                var subject = _mapper.Map<Subject>(createSubjectDTO);

                if (createSubjectDTO.TeacherId == null)
                {
                    return new CustomResponse { Message = "Cannot get teacher value", StatusCode = 400 };
                }

                _context.Subjects.Add(subject);
                await _context.SaveChangesAsync();
                return new CustomResponse { Message =  "Create Subject Success", StatusCode = 200 };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<CustomResponse> UpdateSubjectAsync(int id, CreateSubjectDTO updateSubjectDTO)
        {


            var existingSubject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId == id);

            if (existingSubject == null)
                return new CustomResponse { Message = "Cannot get subject", StatusCode = 400 };
            var subject = _mapper.Map(updateSubjectDTO, existingSubject);

            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();

            return new CustomResponse { Message = "Update Subject Success", StatusCode = 200 };
        }

        public async Task<CustomResponse> DeleteSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId == id);

            if (subject == null)
                return new CustomResponse { Message = "Cannot find this subject", StatusCode = 400 };

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return new CustomResponse { Message = "Delete Subject Success", StatusCode = 200 };
        }
    }
}
