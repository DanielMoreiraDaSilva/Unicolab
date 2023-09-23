using AutoMapper;
using Business.TransferObjects;
using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Mappings
{
    public class DuvidaMapper : Profile
    {
        private readonly IRespostaRepository _respostaRepository;
        public DuvidaMapper(IRespostaRepository respostaRepository)
        {


            CreateMap<Duvida, DuvidaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Respostas, opt => opt.MapFrom(src => GetRespostas(src.Id)));

            CreateMap<DuvidaDto, Duvida>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            _respostaRepository = respostaRepository;
        }

        private List<Resposta> GetRespostas(string duvidaId)
        {
            var result = _respostaRepository.GetAllByDuvidaId(duvidaId);
            Task.WhenAll(result);
            return result.Result;
        }
    }
}
