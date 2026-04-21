using AfetToplanmaAlani.EL.Dtos.Place;
using AfetToplanmaAlani.EL.Dtos.Staff;
using AfetToplanmaAlani.EL.Dtos.WorkGroupStaff;
using AfetToplanmaAlani.EL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AfetToplanmaAlani.WebUI.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StaffDtoForCreate, Staff>().ReverseMap();
            CreateMap<StaffDtoForUpdate, Staff>().ReverseMap();

            CreateMap<WorkGroupStaffDtoForCreate, WorkGroupStaff>().ReverseMap();
            CreateMap<WorkGroupStaffDtoForUpdate, WorkGroupStaff>().ReverseMap();

            CreateMap<PlaceDtoForCreate, Place>().ReverseMap();
            CreateMap<PlaceDtoForUpdate, Place>().ReverseMap();
        }
    }
}
