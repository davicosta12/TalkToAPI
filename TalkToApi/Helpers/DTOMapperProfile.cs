using AutoMapper;
using TalkToApi.V1.Models;
using TalkToApi.V1.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToApi.Helpers
{
    public class DTOMapperProfile : Profile
    {
        public DTOMapperProfile()
        {
            CreateMap<ApplicationUser, UsuarioDTO>()
                .ForMember(dest => dest.Nome,
                orig => orig.MapFrom(src => src.FullName));

            CreateMap<Mensagem, MensagemDTO>();

            CreateMap<ApplicationUser, UsuarioDTOSemHyperLink>()
                 .ForMember(dest => dest.Nome,
                orig => orig.MapFrom(src => src.FullName));
        }
    }
}
