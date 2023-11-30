using GlobalChat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.Data
{
    /// <summary>
    /// En esta clase estará toda la comunicación con la bbdd y la usaremos para acceder a la misma
    /// </summary>
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(DbContextOptions options, IConfiguration configuration): base(options)
        {
            Configuration = configuration;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("WebAPIDatabase"));
            }
        }

        // Agregar tablas

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Configuracion> Configuraciones { get; set; }
        public DbSet<Contactos> Contactos { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
        public DbSet<Sesiones> Sesiones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioChat> Usuarioschats { get; set; }

    }
}
