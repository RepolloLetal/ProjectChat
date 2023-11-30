using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Business.Dtos
{
    public class ContactoCompletoDto
    {
        public int IdOtroUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string UltSesion { get; set; } = string.Empty;
        public bool Favorito { get; set; }
    }
}
