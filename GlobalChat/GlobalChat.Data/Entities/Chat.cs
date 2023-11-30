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
        public string NombreChat { get; set; } = string.Empty;
        public virtual ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
    }
}
