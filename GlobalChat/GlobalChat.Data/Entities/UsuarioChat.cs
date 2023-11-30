using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Data.Entities
{
    [PrimaryKey(nameof(IdUsuario), nameof(IdChat))]
    public record UsuarioChat
    {
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int IdChat {  get; set; }
        public virtual Chat Chat { get; set; }
    }
}
