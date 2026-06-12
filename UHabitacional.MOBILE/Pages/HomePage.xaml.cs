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

    // Todos los módulos llevan a la pantalla "Sitio en construcción".
    // Pasamos el nombre del módulo como parámetro para mostrarlo allí.
    private async void OnModuloTapped(object? sender, TappedEventArgs e)
    {
        string modulo = e.Parameter?.ToString() ?? string.Empty;
        await Shell.Current.GoToAsync($"enconstruccion?modulo={Uri.EscapeDataString(modulo)}");
    }
}
