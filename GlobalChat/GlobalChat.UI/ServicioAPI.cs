using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalChat.UI
{
    public static class ServicioAPI
    {
        public static HttpClient Cliente {  get; private set; }
        private static readonly string URLBASE = "https://localhost:7103/";
        static ServicioAPI()
        {
            Cliente = new HttpClient();
            Cliente.BaseAddress = new Uri(URLBASE);
        }
    }
}
