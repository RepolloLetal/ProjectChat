using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Data.Entities
{
    public record Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreLogin { get; set; }
        public string Password { get; set; }
        public virtual Configuracion Configuracion { get; set; }
        public virtual ICollection<UsuarioChat> UsuarioChat { get; set; }
        public virtual ICollection<Contactos> Contactos { get; set; }
    }
}
