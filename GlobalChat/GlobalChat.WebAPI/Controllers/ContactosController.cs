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
    public class ContactosController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly ServicioAuth auth;
        public ContactosController(DataContext context, IMapper mapper, ServicioAuth auth)
        {
            this.context = context;
            this.mapper = mapper;
            this.auth = auth;
        }

        [HttpPost("AgregarContacto")]
        public async Task<PeticionDto<ContactoDto>> AgregarContacto([FromBody] PeticionDto<NuevoContactoDto> nuevoContacto)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(nuevoContacto.TokenPeticion))
                return new PeticionDto<ContactoDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<ContactoDto> peticionDto = new PeticionDto<ContactoDto>();
            Usuario usuario = context.Usuarios.Where(x => x.NombreLogin == nuevoContacto.Value.NombreContacto).First();

            if (usuario != null)
            {
                peticionDto.PeticionCorrecta = true;
                Contacto contacto = new Contacto();
                contacto.IdUsuarioA = nuevoContacto.Value.IdUsuarioActual;
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

        [HttpPost("EliminarContacto")]
        public async Task<PeticionDto<ContactoDto>> EliminarContacto(PeticionDto<int> idContacto)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(idContacto.TokenPeticion))
                return new PeticionDto<ContactoDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<ContactoDto> peticionDto = new PeticionDto<ContactoDto>();
            Contacto contactoEliminar = context.Contactos.Where(x => x.Id == idContacto.Value).First();

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

        [HttpPost("ObtenerListaContactos")]
        public async Task<PeticionDto<List<ContactoDto>>> ObtenerListaContactos(PeticionDto<int> idUsuario)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(idUsuario.TokenPeticion))
                return new PeticionDto<List<ContactoDto>>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<List<ContactoDto>> peticionDto = new PeticionDto<List<ContactoDto>>();
            List<Contacto> listaContactos = await context.Contactos.Where(x => x.IdUsuarioA == idUsuario.Value || x.IdUsuarioB == idUsuario.Value).ToListAsync();

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
