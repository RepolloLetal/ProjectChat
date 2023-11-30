using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Business.Dtos
{
    public class ContactoDto
    {
        public int Id { get; set; }
        public int IdUsuarioA { get; set; }
        public int IdUsuarioB { get; set; }
    }
}
