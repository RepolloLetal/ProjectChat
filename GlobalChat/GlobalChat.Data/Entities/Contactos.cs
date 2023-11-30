using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Data.Entities
{
    public record Contactos
    {
        public int Id { get; set; }
        [ForeignKey("Usuario")]
        public int IdUsuarioA { get; set; }
        public virtual Usuario UsuarioA { get; set; }
        [ForeignKey("Usuario")]
        public int IdUsuarioB { get; set; }
        public virtual Usuario UsuarioB { get; set; }

    }
}
