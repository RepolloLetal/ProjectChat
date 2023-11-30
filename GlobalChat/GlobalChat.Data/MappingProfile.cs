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
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
        }
    }
}
