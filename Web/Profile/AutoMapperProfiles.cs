using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.Web.Dto.Class;
using iread_school_ms.Web.Dto.School;
using iread_school_ms.Web.Dto.SchoolMembers;
using iread_school_ms.Web.Dto.User;

namespace iread_school_ms.Web.Profile
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            //School Mapper
            CreateMap<School, SchoolDto>().ReverseMap();
            CreateMap<School, InnerSchoolDto>().ReverseMap();
            CreateMap<SchoolCreateDto, School>().ReverseMap();
            CreateMap<SchoolUpdateDto, School>().ReverseMap();

            //Class Mapper
            CreateMap<Class, ClassDto>().ReverseMap();
            CreateMap<Class, InnerClassDto>().ReverseMap();
            CreateMap<Class, ClassCreateDto>().ReverseMap();

            //Class Member Mapper
            CreateMap<ClassMember, StudentDto>().ReverseMap();
            CreateMap<ClassMember, TeacherDto>().ReverseMap();
            CreateMap<ClassMember, InnerClassMemberDto>().ReverseMap();
            CreateMap<ClassMember, SchoolMember>().ReverseMap();

            //School Member Mapper
            CreateMap<SchoolMember, ManagerDto>().ReverseMap();
            CreateMap<SchoolMember, TeacherDto>().ReverseMap();
            CreateMap<SchoolMember, StudentDto>().ReverseMap();
            CreateMap<SchoolMember, InnerSchoolMemberDto>().ReverseMap();
            CreateMap<SchoolMemberDto, SchoolMember>().ReverseMap()
            .ForMember(dest => dest.Id, act => act.MapFrom(src => src.MemberId));
        }
    }
}