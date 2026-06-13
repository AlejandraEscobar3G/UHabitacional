namespace UHabitacional.MOBILE.Pages;

public partial class PerfilesPage : ContentPage
{
    public PerfilesPage()
    {
        InitializeComponent();
    }

    private async void OnRegresarTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnPerfilTapped(object? sender, TappedEventArgs e)
    {
        string perfil = e.Parameter?.ToString() ?? string.Empty;
        await Shell.Current.GoToAsync($"enconstruccion?modulo={Uri.EscapeDataString(perfil)}");
    }
}