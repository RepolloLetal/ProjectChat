using GlobalChat.Data;
using Microsoft.AspNetCore.Mvc;
using GlobalChat.Business.Dtos;
using GlobalChat.Data.Entities;
using AutoMapper;

namespace GlobalChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public UsuarioController(DataContext context, IMapper mapper) 
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("ObtenerUsuario/{id}")]
        public async Task<UsuarioDto> ObtenerUsuario(int id) 
        {
            Usuario usuario = context.Usuarios.Where(x  => x.Id == id).First();

            if (usuario != null) 
            {
                UsuarioDto usuarioDto = mapper.Map<UsuarioDto>(usuario);
                return usuarioDto;
            }
            else
            {
                return new UsuarioDto();
            }
        }

        [HttpPost("RegistrarUsuario")]
        public async Task<UsuarioDto> RegistrarUsuario([FromBody]UsuarioDto nuevoUsuario) 
        {
            Usuario usuario = mapper.Map<Usuario>(nuevoUsuario);
            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();
            return mapper.Map<UsuarioDto>(usuario);
        }

        [HttpPut("LoginUsuario")]
        public async Task<bool> LoginUsuario([FromBody] UsuarioDto usuarioLogin)
        {
            int loginCorrecto = context.Usuarios.Where(x => x.NombreLogin == usuarioLogin.NombreLogin && x.Password == usuarioLogin.Password).Count();
            return loginCorrecto > 0;
        }
    }
}
