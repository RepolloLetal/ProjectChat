using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Business.Dtos
{
    public class ChatDto
    {
        public int Id { get; set; }
        public List<MensajeDto> Mensajes { get; set; } = new List<MensajeDto>();
        public List<UsuarioDto> Usuarios { get; set; } = new List<UsuarioDto>();
    }
}
