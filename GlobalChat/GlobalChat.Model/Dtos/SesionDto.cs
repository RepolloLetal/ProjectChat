using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Business.Dtos
{
    public class SesionDto
    {
        public int Id { get; set; }
        public string DiaHoraSesion { get; set; }
        public int IdUsuario { get; set; }
    }
}
