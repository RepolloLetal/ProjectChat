using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Data.Entities
{
    public record Mensaje
    {
        public int Id { get; set; }
        public string Contenido { get; set; } = string.Empty;
        public string DiaHoraMens { get; set;} = string.Empty;
        public virtual Usuario Usuario { get; set; } = new Usuario();
        public virtual Chat Chat { get; set; } = new Chat();

    }
}
