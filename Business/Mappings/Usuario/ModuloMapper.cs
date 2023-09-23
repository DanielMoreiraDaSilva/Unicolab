using AutoMapper;
using Business.TransferObjects;
using Data.Models;

namespace Business.Mappings
{
    public class ModuloMapper : Profile
    {
        public ModuloMapper()
        {
            CreateMap<Modulo, ModuloDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
