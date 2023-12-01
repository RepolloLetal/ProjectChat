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

        [HttpPost("AgregarUltimaSesion")]
        public async Task<PeticionDto<SesionDto>> AgregarUltimaSesion(PeticionDto<SesionDto> ultSesionPet)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(ultSesionPet.TokenPeticion))
                return new PeticionDto<SesionDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<SesionDto> peticionDto = new PeticionDto<SesionDto>();
            Sesion ultSesion = mapper.Map<Sesion>(ultSesionPet.Value);
            try
            {
                ultSesion.Usuario = context.Usuarios.Where(x => x.Id == ultSesion.IdUsuario).First();
                context.Sesiones.Add(ultSesion);
                await context.SaveChangesAsync();
            }
            catch { }

            if (ultSesion != null)
            {
                peticionDto.PeticionCorrecta = true;
                peticionDto.Value = mapper.Map<SesionDto>(ultSesion);
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "Ultima sesión desconocida";
            }
            return peticionDto;
        }

        [HttpPost("ObtenerUltimaSesion")]
        public async Task<PeticionDto<SesionDto>> ObtenerUltimaSesion(PeticionDto<int> idUsuario)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(idUsuario.TokenPeticion))
                return new PeticionDto<SesionDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<SesionDto> peticionDto = new PeticionDto<SesionDto>();
            Sesion ultimaSesion = context.Sesiones.Where(x => x.IdUsuario == idUsuario.Value).Last();

            if (ultimaSesion != null)
            {
                peticionDto.PeticionCorrecta = true;
                peticionDto.Value = mapper.Map<SesionDto>(ultimaSesion);
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "Ultima sesión desconocida";
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

        [HttpPost("EliminarUsuario")]
        public async Task<PeticionDto<UsuarioDto>> EliminarUsuario([FromBody] PeticionDto<UsuarioDto> EliminaUsuario)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(EliminaUsuario.TokenPeticion))
                return new PeticionDto<UsuarioDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            Usuario usuario = context.Usuarios.Where(x => x.Id == EliminaUsuario.Value.Id).First();
            PeticionDto<UsuarioDto> peticionDto = new PeticionDto<UsuarioDto>();

            if (usuario !=null)
            {
                peticionDto.PeticionCorrecta = true;
                context.Usuarios.Remove(usuario);
                await context.SaveChangesAsync();
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "Cuenta no encontrada, no se pudo eliminar";
            }

            return peticionDto;
        }

    }
}
