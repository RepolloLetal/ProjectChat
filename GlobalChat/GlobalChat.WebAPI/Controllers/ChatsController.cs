using AutoMapper;
using GlobalChat.Business.Dtos;
using GlobalChat.Data;
using GlobalChat.Data.Entities;
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
        public ChatsController(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("ObtenerMensaje/{idMensaje}")]
        public async Task<PeticionDto<MensajeDto>> ObtenerMensaje(int idMensaje)
        {
            PeticionDto<MensajeDto> peticionDto = new PeticionDto<MensajeDto>();
            Mensaje mensaje = context.Mensajes.Where(x => x.Id == idMensaje).First();

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
        [HttpGet("ObtenerListaMensajes/{idChat}")]
        public async Task<PeticionDto<List<MensajeDto>>> ObtenerListaMensajes(int idChat)
        {
            PeticionDto<List<MensajeDto>> peticionDto = new PeticionDto<List<MensajeDto>>();
            List<Mensaje> listaMensajes = await context.Mensajes.Where(x => x.IdChat == idChat).ToListAsync();

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

        [HttpGet("ObtenerListaMensajes/{idSesion}")]
        public async Task<PeticionDto<SesionDto>> ObtenerUltimaSesion(int idSesion)
        {
            PeticionDto<SesionDto> peticionDto = new PeticionDto<SesionDto>();
            Sesion ultimaSesion = context.Sesiones.Where(x => x.Id == idSesion).Last();

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

    
