using AutoMapper;
using RingoMedia.Helper;
using RingoMedia.Models.Domain;
using RingoMedia.Models.Entities;
using RingoMedia.Models.ViewModels;

namespace RingoMedia.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DepartmentEntity, DepartmentVM>()
                .ForMember(dest => dest.SubDepartments, opt => opt.MapFrom(src => src.SubDepartments));
            CreateMap<DepartmentVM, DepartmentEntity>()
                .ForMember(dest => dest.SubDepartments, opt => opt.Ignore());

            CreateMap<ReminderEntity, Reminder>()
                .ForMember(dest => dest.DateTime,
                       opt => opt.MapFrom(src => GenericFunction.ConvertUtcToTimeZone(src.DateTime)));

            CreateMap<Reminder, ReminderEntity>();
        }
    }
}
