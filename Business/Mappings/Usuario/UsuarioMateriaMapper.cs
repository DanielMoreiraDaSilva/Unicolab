using AutoMapper;
using Business.TransferObjects;
using Data.Models;

namespace Business.Mappings
{
    public class UsuarioMateriaMapper : Profile
    {
        public UsuarioMateriaMapper()
        {
            CreateMap<UsuarioMateria, UsuarioMateriaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}