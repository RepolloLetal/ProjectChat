using AutoMapper;
using GlobalChat.Data;
using Microsoft.AspNetCore.Mvc;

namespace GlobalChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public ContactosController(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
    }
}
