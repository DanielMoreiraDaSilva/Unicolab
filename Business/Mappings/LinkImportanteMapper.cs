using AutoMapper;
using Business.TransferObjects;
using Data.Models;

namespace Business.Mappings
{
    public class LinkImportanteMapper : Profile
    {
        public LinkImportanteMapper()
        {
            CreateMap<LinkImportante, LinkImportanteDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
