namespace UHabitacional.MOBILE.Pages;

public partial class CatalogosPage : ContentPage
{
    public CatalogosPage()
    {
        InitializeComponent();
    }

    // Flecha "←": regresa a la pantalla anterior (el Home).
    private async void OnRegresarTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    // Cada catálogo (Tipos de identificación / Perfiles) abre la pantalla
    // "Sitio en construcción", pasándole el nombre del catálogo seleccionado.
    private async void OnCatalogoTapped(object? sender, TappedEventArgs e)
    {
        string catalogo = e.Parameter?.ToString() ?? string.Empty;
        await Shell.Current.GoToAsync($"enconstruccion?modulo={Uri.EscapeDataString(catalogo)}");
    }
}
