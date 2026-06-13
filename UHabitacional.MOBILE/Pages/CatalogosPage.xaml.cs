namespace UHabitacional.MOBILE.Pages;

public partial class CatalogosPage : ContentPage
{
    public CatalogosPage()
    {
        InitializeComponent();
    }

    private async void OnRegresarTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCatalogoTapped(object? sender, TappedEventArgs e)
    {
        string catalogo = e.Parameter?.ToString() ?? string.Empty;

        if (catalogo == "Perfiles")
            await Shell.Current.GoToAsync("perfiles");
        else if (catalogo == "Tipos de identificación")
            await Shell.Current.GoToAsync("tiposidentificacion");
        else
            await Shell.Current.GoToAsync($"enconstruccion?modulo={Uri.EscapeDataString(catalogo)}");
    }
}
