using AutoMapper;
using GlobalChat.Data;
using Microsoft.AspNetCore.Mvc;

namespace GlobalChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {

        private readonly DataContext context;
        private readonly IMapper mapper;
        public ChatsController(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
    }
}
