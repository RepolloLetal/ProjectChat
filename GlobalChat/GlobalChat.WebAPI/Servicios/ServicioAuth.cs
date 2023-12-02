namespace GlobalChat.WebApi.Servicios
{
    public class ServicioAuth
    {
        private static Dictionary<int, string> UsuariosAuth { get; set; } = new Dictionary<int, string>();
        public string Token { get; set; }

        public string UsuarioInicioSesion(int id)
        {
            //Generamos Token unico de usuario
            string token = "";
            byte[] tokenByte = new byte[32];
            Random rnd = new Random();
            rnd.NextBytes(tokenByte);
            token = Convert.ToBase64String(tokenByte);

            //Guardamos usuario en lista de usuarios autenticados para peticiones
            if(UsuariosAuth.ContainsKey(id))
            {
                UsuariosAuth[id] = token;
            }
            else
            {
                UsuariosAuth.Add(id, token);
            }
            return token;
        }

        public bool ComprobarUsuarioValido(string token)
        {
            return UsuariosAuth.ContainsValue(token);
        }
    }
}
