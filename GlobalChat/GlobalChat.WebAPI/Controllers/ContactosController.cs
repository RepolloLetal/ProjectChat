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

        [HttpDelete("EliminarContacto/{id}")]
        public async Task<PeticionDto<ContactoDto>> EliminarContacto(int id)
        {
            PeticionDto<ContactoDto> peticionDto = new PeticionDto<ContactoDto>();
            Contacto contactoEliminar = context.Contactos.Where(x => x.Id == id).First();

            if (contactoEliminar !=null)
            {
                peticionDto.PeticionCorrecta = true;
                context.Contactos.Remove(contactoEliminar);
                await context.SaveChangesAsync();
                peticionDto.Value = mapper.Map<ContactoDto>(contactoEliminar);
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "No se ha logrado borrar el contacto";
            }
            return peticionDto;

        }

        [HttpGet("ObtenerListaContactos/{idUsuario}")]
        public async Task<PeticionDto<List<ContactoDto>>> ObtenerListaContactos(int idUsuario)
        {
            PeticionDto<List<ContactoDto>> peticionDto = new PeticionDto<List<ContactoDto>>();
            List<Contacto> listaContactos = await context.Contactos.Where(x => x.IdUsuarioA == idUsuario || x.IdUsuarioB == idUsuario).ToListAsync();

            if(listaContactos != null && listaContactos.Count > 0)
            {
                peticionDto.PeticionCorrecta = true;
                peticionDto.Value = mapper.Map<List<ContactoDto>>(listaContactos);
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "El usuario no tiene contactos";
            }
            return peticionDto;

        }
    }
}
