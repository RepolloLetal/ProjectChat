using AutoMapper;
using GlobalChat.Business.Dtos;
using GlobalChat.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Chat, ChatDto>().ReverseMap();
            CreateMap<Configuracion, ConfiguracionDto>().ReverseMap();
            CreateMap<Contacto, ContactoDto>().ReverseMap();
            CreateMap<Mensaje, MensajeDto>().ReverseMap();
            CreateMap<Sesion, SesionDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<UsuarioChat, UsuarioChatDto>().ReverseMap();
        }
    }
}
