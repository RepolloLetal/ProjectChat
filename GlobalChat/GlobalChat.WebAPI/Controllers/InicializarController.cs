using GlobalChat.Data;
using Microsoft.AspNetCore.Mvc;

namespace GlobalChat.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InicializarController : Controller
    {
        private readonly DataContext _context;

        public InicializarController(DataContext context)
        {
            _context = context;
            context.Database.EnsureCreated();
        }
        public string Prueba()
        {
            return "Al huevo con pan";
        }
    }
}
