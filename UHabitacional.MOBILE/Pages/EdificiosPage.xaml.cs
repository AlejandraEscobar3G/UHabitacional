using System.Collections.ObjectModel;
using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

public partial class EdificiosPage : ContentPage
{
    private readonly EdificiosService _edificiosService = new();

    public ObservableCollection<Edificio> Edificios { get; } = new();

    public EdificiosPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarEdificiosAsync();
    }

    private async Task CargarEdificiosAsync()
    {
        MensajeLabel.IsVisible = false;
        MostrarCargando(true);

        try
        {
            List<Edificio> edificios = await _edificiosService.ObtenerEdificiosAsync();

            Edificios.Clear();
            foreach (Edificio edificio in edificios)
            {
                Edificios.Add(edificio);
            }

            if (Edificios.Count == 0)
            {
                MostrarMensaje("No hay edificios registrados.");
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

    private async void OnEdificioTapped(object? sender, TappedEventArgs e)
    {
        string edificio = e.Parameter?.ToString() ?? string.Empty;
        await Shell.Current.GoToAsync($"enconstruccion?modulo={Uri.EscapeDataString(edificio)}");
    }
}
