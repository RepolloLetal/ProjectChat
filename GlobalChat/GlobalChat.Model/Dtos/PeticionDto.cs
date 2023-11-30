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
        public string MensajeError { get; set; } = string.Empty;
        public T? Value { get; set; }
        public string TokenPeticion { get; set; } = string.Empty;
        public bool ErrorPorToken { get; set; }
    }

}
