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

        [HttpPost("EnviarMensaje")]
        public async Task<PeticionDto<MensajeDto>> EnviarMensaje(PeticionDto<MensajeDto> mensaje)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(mensaje.TokenPeticion))
                return new PeticionDto<MensajeDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<MensajeDto> peticionDto = new PeticionDto<MensajeDto>();
            try
            {
                Mensaje nuevoMen = mapper.Map<Mensaje>(mensaje.Value);
                nuevoMen.Chat = context.Chats.Where(x => x.Id == nuevoMen.Chat.Id).First();
                nuevoMen.Usuario = context.Usuarios.Where(x => x.Id == nuevoMen.Usuario.Id).First();
                context.Mensajes.Add(nuevoMen);
                await context.SaveChangesAsync();
                peticionDto.PeticionCorrecta = true;
                nuevoMen.Chat.Mensajes = new List<Mensaje>();
                peticionDto.Value = mapper.Map<MensajeDto>(nuevoMen);
            }
            catch(Exception e)
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "Error al enviar el mensaje: " + e.Message;
            }
            return peticionDto;

        }

        [HttpPost("ObtenerMensajesChat")]
        public async Task<PeticionDto<ChatDto>> ObtenerMensajesChat(PeticionDto<ContactoDto> contacto)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(contacto.TokenPeticion))
                return new PeticionDto<ChatDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<ChatDto> peticionDto = new PeticionDto<ChatDto>();
            Chat chat = null;
            try
            {
                chat = context.Chats.Where(x => x.Usuarios.Any(y => y.Id == contacto.Value.IdUsuarioA) && x.Usuarios.Any(y => y.Id == contacto.Value.IdUsuarioB)).First();
            }
            catch { }
            if(chat == null)
            {
                chat = new Chat();
                Usuario usuarioA = context.Usuarios.Where(x => x.Id == contacto.Value.IdUsuarioA).First();
                Usuario usuarioB = context.Usuarios.Where(x => x.Id == contacto.Value.IdUsuarioB).First();
                chat.Usuarios.Add(usuarioA);
                chat.Usuarios.Add(usuarioB);
                context.Chats.Add(chat);
                await context.SaveChangesAsync();
            }
            List<Mensaje> listaMensajes = new List<Mensaje>();
            try
            {
                List<Mensaje> listaMensajesA = context.Mensajes.Where(x => x.Chat.Id == chat.Id && x.Usuario.Id == contacto.Value.IdUsuarioA).ToList();
                listaMensajesA.ForEach(x => x.Usuario.Id = contacto.Value.IdUsuarioA);
                List<Mensaje> listaMensajesB = context.Mensajes.Where(x => x.Chat.Id == chat.Id && x.Usuario.Id == contacto.Value.IdUsuarioB).ToList();
                listaMensajesB.ForEach(x => x.Usuario.Id = contacto.Value.IdUsuarioB);
                listaMensajes.AddRange(listaMensajesA);
                listaMensajes.AddRange(listaMensajesB);
                listaMensajes.ForEach(x => x.Chat = null);
                listaMensajes = listaMensajes.OrderBy(x => x.Id).ToList();
            }
            catch { }

            if (chat != null)
            {
                chat.Mensajes = listaMensajes;
                peticionDto.PeticionCorrecta = true;
                chat.Usuarios = null;
                peticionDto.Value = mapper.Map<ChatDto>(chat);
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "Error al recuperar el chat";
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

    }
}

    
