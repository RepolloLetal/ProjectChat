using AutoMapper;
using GlobalChat.Business.Dtos;
using GlobalChat.Data;
using GlobalChat.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GlobalChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public ContactosController(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost("AgregarContacto")]
        public async Task<PeticionDto<ContactoDto>> AgregarContacto([FromBody] NuevoContactoDto nuevoContacto)
        {
            PeticionDto<ContactoDto> peticionDto = new PeticionDto<ContactoDto>();
            Usuario usuario = context.Usuarios.Where(x => x.NombreLogin == nuevoContacto.NombreContacto).First();

            if (usuario != null)
            {
                peticionDto.PeticionCorrecta = true;
                Contacto contacto = new Contacto();
                contacto.IdUsuarioA = nuevoContacto.IdUsuarioActual;
                contacto.IdUsuarioB = usuario.Id;
                await context.Contactos.AddAsync(contacto);
                await context.SaveChangesAsync();
                peticionDto.Value = mapper.Map<ContactoDto>(contacto);
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "No se ha encontrado el usuario";
            }
            return peticionDto;

        }
    }
}
