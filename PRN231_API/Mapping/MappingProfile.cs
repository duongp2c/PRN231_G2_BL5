namespace PRN231_API.Mapping
{
    using AutoMapper;
    using PRN231_API.DTO;
    using PRN231_API.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Evaluation, EvaluationDTO>();
            CreateMap<EvaluationDTO, Evaluation>();
            CreateMap<Student, StudentDTO>();
            CreateMap<StudentDTO, Student>();
            CreateMap<Subject, SubjectDTO>();
            CreateMap<SubjectDTO, Subject>();
            CreateMap<GradeType, GradeTypeDTO>();
            CreateMap<GradeTypeDTO, GradeType>();
        }
    }

}
