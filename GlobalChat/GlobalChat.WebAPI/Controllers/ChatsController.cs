using AutoMapper;
using GlobalChat.Business.Dtos;
using GlobalChat.Data;
using GlobalChat.Data.Entities;
using GlobalChat.WebApi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobalChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {

        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly ServicioAuth auth;
        public ChatsController(DataContext context, IMapper mapper, ServicioAuth auth)
        {
            this.context = context;
            this.mapper = mapper;
            this.auth = auth;
        }

        [HttpPost("ObtenerMensaje")]
        public async Task<PeticionDto<MensajeDto>> ObtenerMensaje(PeticionDto<int> idMensaje)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(idMensaje.TokenPeticion))
                return new PeticionDto<MensajeDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<MensajeDto> peticionDto = new PeticionDto<MensajeDto>();
            Mensaje mensaje = context.Mensajes.Where(x => x.Id == idMensaje.Value).First();

            if(mensaje != null) 
            {
                peticionDto.PeticionCorrecta = true;
                peticionDto.Value = mapper.Map<MensajeDto>(mensaje);
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "No se ha encontrado el mensaje";
            }
            return peticionDto;

        }

        [HttpPost("ObtenerListaMensajes")]
        public async Task<PeticionDto<List<MensajeDto>>> ObtenerListaMensajes(PeticionDto<int> idChat)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(idChat.TokenPeticion))
                return new PeticionDto<List<MensajeDto>>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<List<MensajeDto>> peticionDto = new PeticionDto<List<MensajeDto>>();
            List<Mensaje> listaMensajes = await context.Mensajes.Where(x => x.IdChat == idChat.Value).ToListAsync();

            if (listaMensajes != null && listaMensajes.Count > 0)
            {
                peticionDto.PeticionCorrecta = true;
                peticionDto.Value = mapper.Map<List<MensajeDto>>(listaMensajes);
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "El usuario no tiene contactos";
            }
            return peticionDto;
        }

        [HttpPost("ObtenerUltimaSesion")]
        public async Task<PeticionDto<SesionDto>> ObtenerUltimaSesion(PeticionDto<int> idSesion)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(idSesion.TokenPeticion))
                return new PeticionDto<SesionDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<SesionDto> peticionDto = new PeticionDto<SesionDto>();
            Sesion ultimaSesion = context.Sesiones.Where(x => x.Id == idSesion.Value).Last();

            if (ultimaSesion != null)
            {
                peticionDto.PeticionCorrecta = true;
                peticionDto.Value = mapper.Map<SesionDto>(ultimaSesion);
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "Ultima seseión desconocida";
            }
            return peticionDto;

        }

    }
}

    // Obtener lista de mensajes con una persona. Ultima sesion del usuario.

    
