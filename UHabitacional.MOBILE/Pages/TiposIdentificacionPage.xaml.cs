using System.Collections.ObjectModel;
using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

public partial class TiposIdentificacionPage : ContentPage
{
    private readonly TiposIdentificacionService _tiposService = new();

    public ObservableCollection<Identificacion> Tipos { get; } = new();

    public TiposIdentificacionPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarTiposAsync();
    }

    private async Task CargarTiposAsync()
    {
        MensajeLabel.IsVisible = false;
        MostrarCargando(true);

        try
        {
            List<Identificacion> tipos = await _tiposService.ObtenerTiposIdentificacionAsync();

            Tipos.Clear();
            foreach (Identificacion tipo in tipos)
            {
                Tipos.Add(tipo);
            }

            if (Tipos.Count == 0)
            {
                MostrarMensaje("No hay tipos de identificación registrados.");
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

    private async void OnTipoTapped(object? sender, TappedEventArgs e)
    {
        string tipo = e.Parameter?.ToString() ?? string.Empty;
        await Shell.Current.GoToAsync($"enconstruccion?modulo={Uri.EscapeDataString(tipo)}");
    }
}
