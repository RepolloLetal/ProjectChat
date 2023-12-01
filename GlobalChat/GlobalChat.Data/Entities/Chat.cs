using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Data.Entities
{
    public record Chat
    {
        public int Id { get; set; }
        public virtual ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
