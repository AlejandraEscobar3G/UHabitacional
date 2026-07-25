using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    // Cada vez que se muestra la pantalla, actualizamos el saludo con el nombre del usuario.
    protected override void OnAppearing()
    {
        base.OnAppearing();

        string nombre = string.IsNullOrWhiteSpace(SessionService.Nombre)
            ? "Usuario"
            : SessionService.Nombre;

        SaludoLabel.Text = $"Hola, {nombre} 👋";
    }

    private async void OnModuloTapped(object? sender, TappedEventArgs e)
    {
        string modulo = e.Parameter?.ToString() ?? string.Empty;

        if (modulo == "Catálogos")
        {
            await Shell.Current.GoToAsync("catalogos");
        }
        else if (modulo == "Vigilantes")
        {
            await Shell.Current.GoToAsync("vigilantes");
        }
        else if (modulo == "Edificios")
        {
            await Shell.Current.GoToAsync("edificios");
        }
        else if (modulo == "Inquilinos")
        {
            await Shell.Current.GoToAsync("inquilinos");
        }
        else if (modulo == "Departamentos")
        {
            await Shell.Current.GoToAsync("departamentos");
        }
        else if (modulo == "Visitantes")
        {
            await Shell.Current.GoToAsync("visitantes");
        }
        else if (modulo == "Checador")
        {
            await Shell.Current.GoToAsync("checador");
        }
        else if (modulo == "RegistroVisitante")
        {
            await Shell.Current.GoToAsync("registrovisitante");
        }
        else
        {
            await Shell.Current.GoToAsync($"enconstruccion?modulo={Uri.EscapeDataString(modulo)}");
        }
    }
}
