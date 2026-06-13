namespace UHabitacional.MOBILE.Pages;

// Recibe el nombre del módulo a través de la navegación (?modulo=...).
[QueryProperty(nameof(Modulo), "modulo")]
public partial class EnConstruccionPage : ContentPage
{
    private string _modulo = string.Empty;

    public string Modulo
    {
        get => _modulo;
        set
        {
            _modulo = value;
            // Cuando llega el parámetro, actualizamos los textos de la pantalla.
            TituloLabel.Text = value;
            MensajeLabel.Text = $"El módulo \"{value}\" estará disponible próximamente.";
        }
    }

    public EnConstruccionPage()
    {
        InitializeComponent();
    }

    private async void OnRegresarTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
