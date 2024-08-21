using AutoMapper;
using PRN231_API.DTO;
using PRN231_API.Models;

namespace PRN231_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Mapping for Student and StudentDTO
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.StudentDetail.Address))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.StudentDetail.Phone))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.StudentDetail.Image))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Account.IsActive))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.StudentSubjects.Select(ss => ss.Subject)));

            // Mapping for Subject and SubjectDTO
            CreateMap<Subject, SubjectDTO>()
             .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => src.Teacher)); // Handle single Teacher mapping
            // Mapping for Teacher and TeacherDTO
            CreateMap<Teacher, TeacherDTO>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.TeacherName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Account.IsActive))
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.Subjects));

            // Mapping from CreateTeacherDTO to Teacher and Account
            CreateMap<CreateTeacherDTO, Teacher>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.TeacherName))
                .ForMember(dest => dest.AccountId, opt => opt.Ignore());
                 // Map SubjectIds to Subject list; // account will be created separately

            CreateMap<CreateTeacherDTO, Account>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
            // Mapping from UpdateTeacherDTO to Teacher
            CreateMap<UpdateTeacherDTO, Teacher>()
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.SubjectIds.Select(id => new Subject { SubjectId = id }).ToList())); // Update SubjectIds to Subject list
            // Mapping from CreateSubjectDTO to Subject
            CreateMap<CreateSubjectDTO, Subject>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.SubjectName))
                .ForMember(dest => dest.Teacher, opt => opt.Ignore()); // Ignore Teachers if setting separately
        }
    }
}
