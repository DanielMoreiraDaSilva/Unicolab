using AutoMapper;
using Business.TransferObjects;
using Data.Models;

namespace Business.Mappings
{
    public class UsuarioCursoMapper : Profile
    {
        public UsuarioCursoMapper()
        {
            CreateMap<UsuarioCurso, UsuarioCursoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
