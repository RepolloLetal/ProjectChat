﻿using GlobalChat.Business.Dtos;
using GlobalChat.UI.VentanasEmergentes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GlobalChat.UI
{
    public static class ServicioAPI
    {
        public static HttpClient Cliente { get; private set; }
        private static readonly string URLBASE = "https://pigeonchat.azurewebsites.net/";

        public static UsuarioDto Usuario { get; private set;}
        public static string TokenUsuario { get; private set;}

        static ServicioAPI()
        {
            Cliente = new HttpClient();
            Cliente.BaseAddress = new Uri(URLBASE);
        }

        public static void UsuarioLoggeado(PeticionDto<UsuarioDto> petLoginUsuario)
        {
            Usuario = petLoginUsuario.Value;
            TokenUsuario = petLoginUsuario.TokenPeticion;
            UsuarioIniSesion?.Invoke(Usuario, EventArgs.Empty);
        }

        public static event EventHandler UsuarioIniSesion;

        public async static Task ReLogin(INavigation navigation)
        {
            if (ServicioPersistencia.SesionIniciada)
            {
                UsuarioDto usuarioLogin = new UsuarioDto() { NombreLogin = ServicioPersistencia.Usuario, Password = ServicioPersistencia.Clave };
                HttpResponseMessage respuesta = await Cliente.PostAsJsonAsync("api/Usuario/LoginUsuario", usuarioLogin, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                if (respuesta.IsSuccessStatusCode)
                {
                    string respuestaStr = await respuesta.Content.ReadAsStringAsync();
                    PeticionDto<UsuarioDto> petUsu = JsonSerializer.Deserialize<PeticionDto<UsuarioDto>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<UsuarioDto>();
                    if (petUsu.PeticionCorrecta)
                    {
                        UsuarioLoggeado(petUsu);
                        await EnviarUltimaSesion();
                    }
                    else
                    {
                        await navigation.PushModalAsync(new VentanaLogin());
                    }
                }
                else
                {
                    await navigation.PushModalAsync(new VentanaLogin());
                }
            }
            else
            {
                await navigation.PushModalAsync(new VentanaLogin());
            }
        }

        public async static Task EnviarUltimaSesion()
        {
            SesionDto sesion = new SesionDto { DiaHoraSesion = DateTime.Now.ToString("G"), IdUsuario = Usuario.Id };
            PeticionDto<SesionDto> peticionSesion = new PeticionDto<SesionDto>();
            peticionSesion.TokenPeticion = TokenUsuario;
            peticionSesion.Value = sesion;
            await Cliente.PostAsJsonAsync("api/Usuario/AgregarUltimaSesion", peticionSesion, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        public async static Task CerrarSesion(INavigation navigation)
        {
            Usuario = new UsuarioDto();
            TokenUsuario = string.Empty;
            await navigation.PushModalAsync(new VentanaLogin());
        }
    }
}
