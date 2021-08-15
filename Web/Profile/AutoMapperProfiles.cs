using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.Web.Dto.Class;
using iread_school_ms.Web.Dto.School;

namespace iread_school_ms.Web.Profile
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            //School Mapper
            CreateMap<School, SchoolDto>().ReverseMap();
            CreateMap<SchoolCreateDto, School>().ReverseMap();
            CreateMap<SchoolUpdateDto, School>().ReverseMap();

            //Class Mapper
            CreateMap<Class, ClassDto>().ReverseMap();
        }
    }
}