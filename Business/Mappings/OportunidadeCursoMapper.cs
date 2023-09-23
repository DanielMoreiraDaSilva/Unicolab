using AutoMapper;
using Business.TransferObjects;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Mappings
{
    public class OportunidadeCursoMapper : Profile
    {
        public OportunidadeCursoMapper()
        {
            CreateMap<OportunidadeCurso, OportunidadeCursoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
