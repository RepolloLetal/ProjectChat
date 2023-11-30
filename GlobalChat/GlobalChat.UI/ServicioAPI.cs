using GlobalChat.Business.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.UI
{
    public static class ServicioAPI
    {
        public static HttpClient Cliente { get; private set; }
        private static readonly string URLBASE = "https://localhost:7103/";

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
        }
    }
}
