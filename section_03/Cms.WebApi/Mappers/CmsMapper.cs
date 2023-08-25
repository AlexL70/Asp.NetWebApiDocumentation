using AutoMapper;
using Cms.Data.Repository.Models;
using Cms.WebApi.DTOs;

#pragma warning disable CS1591
namespace Cms.WebApi.Mappers
{
    public class CmsMapper : Profile
    {
        public CmsMapper()
        {
            CreateMap<CourseDto, Course>()
                .ReverseMap();

            CreateMap<StudentDto, Student>()
                .ReverseMap();
        }
    }
}
#pragma warning restore CS1591