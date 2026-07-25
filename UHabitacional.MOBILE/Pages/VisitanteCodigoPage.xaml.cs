namespace UHabitacional.MOBILE.Pages;

[QueryProperty(nameof(CodigoParam), "codigo")]
[QueryProperty(nameof(NombreParam), "nombre")]
public partial class VisitanteCodigoPage : ContentPage
{
    private string _codigo = string.Empty;

    public string CodigoParam
    {
        set
        {
            _codigo = value;
            CodigoLabel.Text = value;
        }
    }

    public string NombreParam
    {
        set => NombreLabel.Text = $"Visita para {value} registrada";
    }

    public VisitanteCodigoPage()
    {
        InitializeComponent();
    }

    private async void OnCopiarTapped(object? sender, TappedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(_codigo))
        {
            await Clipboard.SetTextAsync(_codigo);
            CopiadoLabel.IsVisible = true;
        }
    }

    private async void OnVerVisitantesClicked(object? sender, EventArgs e)
    {
        // Pila actual: lista → form → codigo; retroceder dos niveles llega a la lista
        await Shell.Current.GoToAsync("../..");
    }

    private async void OnOtroVisitanteClicked(object? sender, EventArgs e)
    {
        // Retroceder a la lista y abrir un formulario nuevo
        await Shell.Current.GoToAsync("../../visitantes/form");
    }
}
