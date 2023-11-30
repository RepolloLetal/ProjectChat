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
        public string NombreChat { get; set; }
        public virtual ICollection<Mensaje> Mensaje { get; set; }
    }
}
