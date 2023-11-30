using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Data.Entities
{
    public record Contacto
    {
        public int Id { get; set; }
        public int IdUsuarioA { get; set; }
        public int IdUsuarioB { get; set; }

    }
}
