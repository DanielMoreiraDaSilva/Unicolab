using AutoMapper;
using Business.TransferObjects;
using Data.Models;

namespace Business.Mappings
{
    public class PerfilMapper : Profile
    {
        public PerfilMapper()
        {
            CreateMap<Perfil, PerfilDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
