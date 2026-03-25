using Microsoft.JSInterop;
using MudBlazor;

namespace PortalInfraestructura.Web.UI.Temas
{
    public class TemaService(IJSRuntime jsRuntime)
    {
        public MudTheme ConfiguracionTema => new()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "rgb(17, 21, 46)",
                Secondary = "#FF4081",
                AppbarBackground = "rgb(17, 21, 46)",
            },
            PaletteDark = new PaletteDark()
            {
                Primary = "#f2CB3A",
                PrimaryContrastText = "#4A4A4A",
                Secondary = "#FF4081",
                AppbarBackground = "#27272f",
                Background = "#1a1a1f",
                Surface = "#27272f",
                DrawerBackground = "#27272f",
            }
        };
        private readonly IJSRuntime _jsRuntime = jsRuntime;
        public event Action<Tema>? OnTemaCambiado;

        public async Task InicializarTemaAsync()
        {
            Tema temaActual = await ObtenerTemaActualAsync();
            OnTemaCambiado?.Invoke(temaActual);
        }

        public async Task SeleccionarTema(Tema nuevoTema)
        {
            var temaActual = await ObtenerTemaActualAsync();

            if (temaActual != nuevoTema)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "tema", nuevoTema.ToString());
                OnTemaCambiado?.Invoke(nuevoTema);
            }
        }

        public async Task<Tema> ObtenerTemaActualAsync()
        {
            Tema temaActual = Tema.Claro; // Valor predeterminado

            if (Enum.TryParse<Tema>(await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "tema"), true, out var temaActualLS))
            {
                temaActual = temaActualLS;
            }

            return temaActual;
        }

    }
}
