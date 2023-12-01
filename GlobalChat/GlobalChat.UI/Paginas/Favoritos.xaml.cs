#if WINDOWS
using Windows.UI.ViewManagement.Core;
#endif
using GlobalChat.Business.Dtos;
using GlobalChat.UI.Componentes;
using System.Net.Http.Json;
using System.Text.Json;
using System.Timers;

namespace GlobalChat.UI.Paginas;

public partial class Favoritos : ContentPage
{
    private List<ContactoCompletoDto> contactosCompletos = new List<ContactoCompletoDto>();
    private List<ContactoComp> contactosCompletosComp = new List<ContactoComp>();
    private ChatDto chatActual = new ChatDto();
    private ContactoCompletoDto contactoAc = new ContactoCompletoDto();
    private System.Timers.Timer timer;

    public Favoritos()
    {
        InitializeComponent();
        CargarContactos();
        timer = new System.Timers.Timer();
        timer.Elapsed += CargarMensajes;
        timer.Interval = 3000;
        timer.Enabled = true;
        timer.Start();
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        foreach (ContactoComp componente in contactosCompletosComp)
        {
            if (componente.ContactoCompleto.NombreUsuario.ToLower().Contains(e.NewTextValue.ToLower()))
            {
                componente.IsVisible = true;
            }
            else
            {
                componente.IsVisible = false;
            }
        }
    }

    private void ContentPage_Disappearing(object sender, EventArgs e)
    {
        timer.Stop();
        timer.Dispose();
    }

    public async void CargarMensajes(object? sender, ElapsedEventArgs e)
    {
        if (contactoAc != null)
        {
            ChatDto chat = await ObtenerMensajes(contactoAc);
            foreach (MensajeDto mensaje in chat.Mensajes)
            {
                if (!chatActual.Mensajes.Exists(x => x.Id == mensaje.Id))
                {
                    BurbujaChat BurMen = new BurbujaChat(mensaje, mensaje.Usuario.Id == ServicioAPI.Usuario.Id);
                    await MainThread.InvokeOnMainThreadAsync(async () => Mensajes.Add(BurMen));
                }
            }
            chatActual = chat;
        }
    }

    private async void CargarContactos()
    {
        PeticionDto<int> peticionEdi = new PeticionDto<int>() { TokenPeticion = ServicioAPI.TokenUsuario, Value = ServicioAPI.Usuario.Id };
        HttpResponseMessage respuesta = await ServicioAPI.Cliente.PostAsJsonAsync("api/Contactos/ObtenerListaContactos", peticionEdi, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (respuesta.IsSuccessStatusCode)
        {
            string respuestaStr = await respuesta.Content.ReadAsStringAsync();
            PeticionDto<List<ContactoCompletoDto>> petContactos = JsonSerializer.Deserialize<PeticionDto<List<ContactoCompletoDto>>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<List<ContactoCompletoDto>>();
            if (petContactos.PeticionCorrecta)
            {
                contactosCompletos = petContactos.Value.FindAll(x => x.Favorito);
                MostrarContactos();
            }
            else if (petContactos.ErrorPorToken)
            {
                await ServicioAPI.ReLogin(Navigation);
                await DisplayAlert("Error", "Error en la comunicacion con el servidor, intentelo de nuevo.", "OK");
            }
            else
            {
                await DisplayAlert("Error", petContactos.MensajeError, "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Error al conectar con el servidor.", "OK");
        }
    }

    private void MostrarContactos()
    {
        elementosMenu.Clear();
        contactosCompletosComp.Clear();
        foreach (var contacto in contactosCompletos)
        {
            ContactoComp comp = new ContactoComp(contacto);
            comp.Selected += ContactoSeleccionado;
            elementosMenu.Add(comp);
            contactosCompletosComp.Add(comp);
        }
        if (contactosCompletos.Any())
        {
            ContactoSeleccionado(contactosCompletos[0], EventArgs.Empty);
        }
    }

    private async void ContactoSeleccionado(object? sender, EventArgs e)
    {
        contactoAc = sender as ContactoCompletoDto;
        Mensajes.Clear();
        if (contactoAc != null)
        {
            NombreContacto.Text = contactoAc.NombreUsuario;
            chatActual = await ObtenerMensajes(contactoAc);
            foreach (MensajeDto mensaje in chatActual.Mensajes)
            {
                BurbujaChat BurMen = new BurbujaChat(mensaje, mensaje.Usuario.Id == ServicioAPI.Usuario.Id);
                Mensajes.Add(BurMen);
            }
        }
    }

    private void EnvMensaje_Clicked(object sender, EventArgs e)
    {
        EnviarMensaje();
    }

    private async void EnviarMensaje()
    {
        MensajeDto nuevoMen = new MensajeDto();
        nuevoMen.Contenido = textInput.Text;
        nuevoMen.DiaHoraMens = DateTime.Now.ToString("G");
        nuevoMen.Usuario = ServicioAPI.Usuario;
        nuevoMen.Chat = new ChatDto() { Id = chatActual.Id };
        PeticionDto<MensajeDto> mensaje = new PeticionDto<MensajeDto>() { TokenPeticion = ServicioAPI.TokenUsuario, Value = nuevoMen };
        HttpResponseMessage respuesta = await ServicioAPI.Cliente.PostAsJsonAsync("api/Chats/EnviarMensaje", mensaje, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (respuesta.IsSuccessStatusCode)
        {
            string respuestaStr = await respuesta.Content.ReadAsStringAsync();
            PeticionDto<MensajeDto> nuevoMenPet = JsonSerializer.Deserialize<PeticionDto<MensajeDto>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<MensajeDto>();
            if (nuevoMenPet.PeticionCorrecta)
            {
                textInput.Text = "";
                chatActual.Mensajes.Add(nuevoMenPet.Value);
                BurbujaChat BurMen = new BurbujaChat(nuevoMenPet.Value, nuevoMenPet.Value.Usuario.Id == ServicioAPI.Usuario.Id);
                Mensajes.Add(BurMen);
            }
            else if (nuevoMenPet.ErrorPorToken)
            {
                await ServicioAPI.ReLogin(Navigation);
                await DisplayAlert("Error", "Error en la comunicacion con el servidor, intentelo de nuevo.", "OK");
            }
            else
            {
                await DisplayAlert("Error", nuevoMenPet.MensajeError, "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Error al conectar con el servidor.", "OK");
        }
    }

    private void Emoticon_Clicked(object sender, EventArgs e)
    {
#if WINDOWS
        textInput.Focus();
        CoreInputView.GetForCurrentView().TryShow(CoreInputViewKind.Emoji);
        textInput.Focus();
#else
        textInput.Focus();
#endif
    }

    private async Task<ChatDto> ObtenerMensajes(ContactoCompletoDto contacto)
    {
        ChatDto chat = new ChatDto();
        ContactoDto contactoChat = new ContactoDto();
        contactoChat.IdUsuarioA = ServicioAPI.Usuario.Id;
        contactoChat.IdUsuarioB = contacto.IdOtroUsuario;
        PeticionDto<ContactoDto> petChat = new PeticionDto<ContactoDto>() { TokenPeticion = ServicioAPI.TokenUsuario, Value = contactoChat };
        HttpResponseMessage respuesta = await ServicioAPI.Cliente.PostAsJsonAsync("api/Chats/ObtenerMensajesChat", petChat, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (respuesta.IsSuccessStatusCode)
        {
            string respuestaStr = await respuesta.Content.ReadAsStringAsync();
            PeticionDto<ChatDto> petContactos = JsonSerializer.Deserialize<PeticionDto<ChatDto>>(respuestaStr, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new PeticionDto<ChatDto>();
            if (petContactos.PeticionCorrecta)
            {
                chat = petContactos.Value;
            }
            else if (petContactos.ErrorPorToken)
            {
                await ServicioAPI.ReLogin(Navigation);
                await DisplayAlert("Error", "Error en la comunicacion con el servidor, intentelo de nuevo.", "OK");
            }
            else
            {
                await DisplayAlert("Error", petContactos.MensajeError, "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Error al conectar con el servidor.", "OK");
        }
        return chat;
    }
}