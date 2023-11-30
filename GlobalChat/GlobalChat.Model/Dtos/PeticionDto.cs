using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Business.Dtos
{
    
    public class PeticionDto<T>
    {
        public bool PeticionCorrecta { get; set; }
        public string MensajeError { get; set; }
        public T Value { get; set; }
    }

}
