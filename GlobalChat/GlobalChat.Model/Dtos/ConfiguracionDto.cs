﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Business.Dtos
{
    public class ConfiguracionDto
    {
        public int Id { get; set; }
        public string JsonConfig { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
    }
}
