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
        public PeticionDto<UsuarioDto> ObtenerUsuario(int id) 
        {
            Usuario usuario = context.Usuarios.Where(x  => x.Id == id).First();
            PeticionDto<UsuarioDto> peticionDto = new PeticionDto<UsuarioDto>();

            if (usuario != null) 
            {
                UsuarioDto usuarioDto = mapper.Map<UsuarioDto>(usuario);
                peticionDto.PeticionCorrecta = true;
                peticionDto.Value = usuarioDto;
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "Usuario no encontrado";
            }
            return peticionDto;
        }

        [HttpPost("RegistrarUsuario")]
        public async Task<PeticionDto<UsuarioDto>> RegistrarUsuario([FromBody]UsuarioDto nuevoUsuario) 
        {
            Usuario usuario = mapper.Map<Usuario>(nuevoUsuario);
            PeticionDto<UsuarioDto> peticionDto = new PeticionDto<UsuarioDto>();

            if (context.Usuarios.Where(x => x.NombreLogin == nuevoUsuario.NombreLogin).Any())
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "Nombre de usuario ya registrado";
            }
            else
            {
                await context.Usuarios.AddAsync(usuario);
                await context.SaveChangesAsync();
                UsuarioDto nuevoUsuarioDto = mapper.Map<UsuarioDto>(usuario);
                peticionDto.Value = nuevoUsuarioDto;
                peticionDto.PeticionCorrecta = true;
            }
 
            return peticionDto;
        }

        [HttpPut("LoginUsuario")]
        public PeticionDto<UsuarioDto> LoginUsuario([FromBody] UsuarioDto usuarioLogin)
        {
            PeticionDto<UsuarioDto> peticionDto = new PeticionDto<UsuarioDto>();
            peticionDto.PeticionCorrecta = context.Usuarios.Where(x => x.NombreLogin == usuarioLogin.NombreLogin && x.Password == usuarioLogin.Password).Any();
            if (!peticionDto.PeticionCorrecta)
            {
                peticionDto.MensajeError = "Usuario y/o contraseña incorrectos";
            }
            peticionDto.Value = usuarioLogin;
            return peticionDto;
        }

        [HttpGet("ObtenerConfiguracion/{idUsuario}")]
        public PeticionDto<ConfiguracionDto> ObtenerConfiguracion(int idUsuario)
        {
            PeticionDto<ConfiguracionDto> peticionDto = new PeticionDto<ConfiguracionDto>();
            Configuracion config = context.Configuraciones.Where(x => x.IdUsuario == idUsuario).First();

            if (config != null)
            {
                peticionDto.PeticionCorrecta = true;
                peticionDto.Value = mapper.Map<ConfiguracionDto>(config);
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "No se ha encontrado la configuración del usuario";
            }
            return peticionDto;
        }
    }
}
