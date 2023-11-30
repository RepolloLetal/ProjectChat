using AutoMapper;
using GlobalChat.Business.Dtos;
using GlobalChat.Data;
using GlobalChat.Data.Entities;
using GlobalChat.WebApi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
            Usuario usuario = null;
            try
            {
                usuario = context.Usuarios.Where(x => x.Nombre == nuevoContacto.Value.NombreContacto).First();
            }
            catch { }

            if (usuario != null)
            {
                bool agregado = context.Contactos.Where(x => x.IdUsuarioA == nuevoContacto.Value.IdUsuarioActual && x.IdUsuarioB == usuario.Id).Any();
                if (!agregado)
                {
                    peticionDto.PeticionCorrecta = true;
                    Contacto contacto = new Contacto();
                    contacto.IdUsuarioA = nuevoContacto.Value.IdUsuarioActual;
                    contacto.IdUsuarioB = usuario.Id;
                    contacto.Favorito = false;
                    await context.Contactos.AddAsync(contacto);
                    await context.SaveChangesAsync();
                    peticionDto.Value = mapper.Map<ContactoDto>(contacto);
                }
                else
                {
                    peticionDto.PeticionCorrecta = false;
                    peticionDto.MensajeError = "Usuario ya agregado";
                }
            }
            else
            {
                peticionDto.PeticionCorrecta = false;
                peticionDto.MensajeError = "No se ha encontrado el usuario";
            }
            return peticionDto;
        }

        [HttpPost("EliminarContacto")]
        public async Task<PeticionDto<ContactoDto>> EliminarContacto(PeticionDto<ContactoDto> contactoEli)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(contactoEli.TokenPeticion))
                return new PeticionDto<ContactoDto>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<ContactoDto> peticionDto = new PeticionDto<ContactoDto>();
            Contacto contactoEliminar = context.Contactos.Where(x => x.IdUsuarioA == contactoEli.Value.IdUsuarioA && x.IdUsuarioB == contactoEli.Value.IdUsuarioB).First();

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
        public async Task<PeticionDto<List<ContactoCompletoDto>>> ObtenerListaContactos(PeticionDto<int> idUsuario)
        {
            //Comprobacion usuario valido
            if (!auth.ComprobarUsuarioValido(idUsuario.TokenPeticion))
                return new PeticionDto<List<ContactoCompletoDto>>() { PeticionCorrecta = false, ErrorPorToken = true };

            PeticionDto<List<ContactoCompletoDto>> peticionDto = new PeticionDto<List<ContactoCompletoDto>>();
            List<Contacto> listaContactos = await context.Contactos.Where(x => x.IdUsuarioA == idUsuario.Value).ToListAsync();

            if(listaContactos != null && listaContactos.Count > 0)
            {
                List<ContactoCompletoDto> contactosCompletos = new List<ContactoCompletoDto>();
                foreach (Contacto con in listaContactos)
                {
                    ContactoCompletoDto nuevoCon = new ContactoCompletoDto();
                    nuevoCon.IdOtroUsuario = con.IdUsuarioB;
                    nuevoCon.NombreUsuario = context.Usuarios.Where(x => x.Id == nuevoCon.IdOtroUsuario).First().Nombre;
                    try
                    {
                        nuevoCon.UltSesion = context.Sesiones.Where(x => x.IdUsuario == nuevoCon.IdOtroUsuario).Last().DiaHoraSesion;
                    }
                    catch 
                    {
                        nuevoCon.UltSesion = "Nunca conectado";
                    }
                    nuevoCon.Favorito = con.Favorito;
                    contactosCompletos.Add(nuevoCon);
                }
                peticionDto.PeticionCorrecta = true;
                peticionDto.Value = contactosCompletos;
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
