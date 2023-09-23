using AutoMapper;
using Business.TransferObjects;
using Data.Models;

namespace Business.Mappings
{
    public class EventoCursoMapper : Profile
    {
        public EventoCursoMapper()
        {
            CreateMap<EventoCurso, EventoCursoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
