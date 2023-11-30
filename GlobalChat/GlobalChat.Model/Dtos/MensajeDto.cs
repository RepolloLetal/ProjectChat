using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Business.Dtos
{
    public class MensajeDto
    {
        public int Id { get; set; }
        public string ContenidoJson { get; set; }
        public string DiaHoraMens { get; set; }
        public int IdUsuario { get; set; }
        public int IdChat { get; set; }

    }
}
