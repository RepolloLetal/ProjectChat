using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Business.Dtos
{
    public class MensajeDto
    {
        public int Id { get; set; }
        public string Contenido { get; set; } = string.Empty;
        public string DiaHoraMens { get; set; } = string.Empty;
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public ChatDto Chat { get; set; } = new ChatDto();

    }
}
