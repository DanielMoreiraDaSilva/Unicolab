using AutoMapper;
using Business.TransferObjects;
using Data.Models;

namespace Business.Mappings
{
    public class PerfilModuloMapper : Profile
    {
        public PerfilModuloMapper()
        {
            CreateMap<PerfilModulo, PerfilModuloDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
