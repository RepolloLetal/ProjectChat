using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Business.Dtos
{
    public class UsuarioChatDto
    {
        public int IdUsuario { get; set; }
        public virtual UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public int IdChat { get; set; }
        public virtual ChatDto Chat { get; set; } = new ChatDto();
    }
}
