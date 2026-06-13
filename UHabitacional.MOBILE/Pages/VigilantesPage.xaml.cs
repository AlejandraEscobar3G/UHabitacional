using System.Collections.ObjectModel;
using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

public partial class VigilantesPage : ContentPage
{
    private readonly VigilantesService _vigilantesService = new();

    public ObservableCollection<Usuario> Vigilantes { get; } = new();

    public VigilantesPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarVigilantesAsync();
    }

    private async Task CargarVigilantesAsync()
    {
        MensajeLabel.IsVisible = false;
        MostrarCargando(true);

        try
        {
            List<Usuario> vigilantes = await _vigilantesService.ObtenerVigilantesAsync();

            Vigilantes.Clear();
            foreach (Usuario vigilante in vigilantes)
            {
                Vigilantes.Add(vigilante);
            }

            if (Vigilantes.Count == 0)
            {
                MostrarMensaje("No hay vigilantes registrados.");
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message);
        }
        finally
        {
            MostrarCargando(false);
        }
    }

    private void MostrarCargando(bool cargando)
    {
        LoadingIndicator.IsVisible = cargando;
        LoadingIndicator.IsRunning = cargando;
    }

    private void MostrarMensaje(string mensaje)
    {
        MensajeLabel.Text = mensaje;
        MensajeLabel.IsVisible = true;
    }

    private async void OnRegresarTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnVigilanteTapped(object? sender, TappedEventArgs e)
    {
        string vigilante = e.Parameter?.ToString() ?? string.Empty;
        await Shell.Current.GoToAsync($"enconstruccion?modulo={Uri.EscapeDataString(vigilante)}");
    }
}
