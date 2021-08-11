using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.Web.Dto.AudioDto;

namespace iread_school_ms.Web.Profile
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            //Audio Mapper
            CreateMap<School, SchoolDto>().ReverseMap();
        }
    }
}