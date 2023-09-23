using AutoMapper;
using Business.TransferObjects;
using Data.Models;

namespace Business.Mappings
{
    public class OportunidadeMapper : Profile
    {
        public OportunidadeMapper()
        {
            CreateMap<Oportunidade, OportunidadeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
