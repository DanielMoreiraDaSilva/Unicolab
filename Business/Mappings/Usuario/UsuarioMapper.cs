using AutoMapper;
using Business.TransferObjects;
using Data.Models;
using Data.Models.Enums;
using Data.Models.Filtros;

namespace Business.Mappings
{
    public class UsuarioMapper : Profile
    {
        public UsuarioMapper()
        {
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
            
            CreateMap<FiltroUsuarioContato, FiltroUsuario>()
                .ForMember(dest => dest.OrdenarPor, opt => opt.MapFrom(src => MapeamentoEnum(src.OrdenarPor)))
                .ReverseMap();

        }
        private CamposUsuarioEnum MapeamentoEnum(CamposUsuarioContatoEnum contatoEnum)
        {
            switch (contatoEnum)
            {
                case CamposUsuarioContatoEnum.Nome:
                    return CamposUsuarioEnum.Nome;
                case CamposUsuarioContatoEnum.Login:
                    return CamposUsuarioEnum.Login;
                case CamposUsuarioContatoEnum.Email:
                    return CamposUsuarioEnum.Email;
                case CamposUsuarioContatoEnum.Status:
                    return CamposUsuarioEnum.Status;
                default:
                    return CamposUsuarioEnum.Nome;
            }
        }
    }
}
