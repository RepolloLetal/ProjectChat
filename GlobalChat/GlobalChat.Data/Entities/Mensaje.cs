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
        public string ContenidoJson { get; set; }
        public string DiaHoraMens { get; set;}
        public int IdUsuario { get; set;}
        public virtual Usuario Usuario { get; set; }
        public int IdChat { get; set; }
        public virtual Chat Chat { get; set; }

    }
}
