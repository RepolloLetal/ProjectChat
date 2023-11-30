using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Data.Entities
{
    public record Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string NombreLogin { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public virtual Configuracion Configuracion { get; set; } = new Configuracion();
        public virtual ICollection<UsuarioChat> UsuarioChat { get; set; } = new List<UsuarioChat>();
    }
}
