﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Business.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string NombreLogin { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<ChatDto> Chats { get; set; } = new List<ChatDto>();
    }
}
