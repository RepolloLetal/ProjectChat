using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Business.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreLogin { get; set; }
        public string Password { get; set; }
        public ConfiguracionDto Configuracion { get; set; }
        public List<ChatDto> Chats { get; set; }
    }
}
