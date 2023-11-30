using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Data.Entities
{
    public record Configuracion
    {
        public int Id { get; set; }
        public string JsonConfig { get; set; }
        // Se crea la relación entre ambas entidades
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set;}
    }

    
}
