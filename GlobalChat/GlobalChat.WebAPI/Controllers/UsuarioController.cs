using GlobalChat.Data;
using Microsoft.AspNetCore.Mvc;
using GlobalChat.Business.Dtos;
using GlobalChat.Data.Entities;
using AutoMapper;
using GlobalChat.WebApi.Servicios;

namespace GlobalChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly ServicioAuth auth;
        public UsuarioController(DataContext context, IMapper mapper, ServicioAuth auth) 
        {
            this.context = context;
            this.mapper = mapper;
            this.auth = auth;
        }

        [HttpPost("ObtenerUsuario")]
        public PeticionDto<UsuarioDto> ObtenerUsuario(PeticionDto<int> idUsuario)
        {
            //Comprobacion usuario valido
            if(!auth.ComprobarUsuarioValido(idUsuario.TokenPeticion)) 
                return new PeticionDto<UsuarioDto>() { PeticionCorrecta = false, ErrorPorToken = true};

            Usuario usuario = context.Usuarios.Where(x  => x.Id == idUsuario.Value).First();
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

        [HttpPost("LoginUsuario")]
        public PeticionDto<UsuarioDto> LoginUsuario([FromBody] UsuarioDto usuarioLogin)
        {
            PeticionDto<UsuarioDto> peticionDto = new PeticionDto<UsuarioDto>();
            peticionDto.PeticionCorrecta = context.Usuarios.Where(x => x.NombreLogin == usuarioLogin.NombreLogin && x.Password == usuarioLogin.Password).Any();
            if (!peticionDto.PeticionCorrecta)
            {
                peticionDto.MensajeError = "Usuario y/o contraseña incorrectos";
            }
            else
            {
                usuarioLogin = mapper.Map<UsuarioDto>(context.Usuarios.Where(x => x.NombreLogin == usuarioLogin.NombreLogin && x.Password == usuarioLogin.Password).First());
                peticionDto.TokenPeticion = auth.UsuarioInicioSesion(usuarioLogin.Id);
            }
            peticionDto.Value = usuarioLogin;
            return peticionDto;
        }

        [HttpPost("EditarUsuario")]
        public async Task<PeticionDto<UsuarioDto>> EditarUsuario([FromBody] PeticionDto<UsuarioDto> usuarioEdicion)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(usuarioEdicion.TokenPeticion))
                return new PeticionDto<UsuarioDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            Usuario usuario = mapper.Map<Usuario>(usuarioEdicion.Value);
            usuario.Configuracion = null;
            usuario.UsuarioChat = null;
            PeticionDto<UsuarioDto> peticionDto = new PeticionDto<UsuarioDto>();

            if (context.Usuarios.Where(x => x.NombreLogin == usuario.NombreLogin && x.Id != usuario.Id).Any())
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "Nombre de usuario en uso";
            }
            else
            {
                context.Usuarios.Update(usuario);
                await context.SaveChangesAsync();
                UsuarioDto usuarioEdicionDto = mapper.Map<UsuarioDto>(usuario);
                peticionDto.Value = usuarioEdicionDto;
                peticionDto.PeticionCorrecta = true;
            }

            return peticionDto;
        }

        [HttpPost("ObtenerConfiguracion")]
        public PeticionDto<ConfiguracionDto> ObtenerConfiguracion(PeticionDto<int> idUsuario)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(idUsuario.TokenPeticion))
                return new PeticionDto<ConfiguracionDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<ConfiguracionDto> peticionDto = new PeticionDto<ConfiguracionDto>();
            Configuracion config = context.Configuraciones.Where(x => x.IdUsuario == idUsuario.Value).First();

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
