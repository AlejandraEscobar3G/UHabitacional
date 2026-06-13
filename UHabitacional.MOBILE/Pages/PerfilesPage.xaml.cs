using System.Collections.ObjectModel;
using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

public partial class PerfilesPage : ContentPage
{
    private readonly PerfilesService _perfilesService = new();

    public ObservableCollection<Perfil> Perfiles { get; } = new();

    public PerfilesPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarPerfilesAsync();
    }

    private async Task CargarPerfilesAsync()
    {
        MostrarCargando(true);

        try
        {
            List<Perfil> perfiles = await _perfilesService.ObtenerPerfilesAsync();

            Perfiles.Clear();
            foreach (Perfil perfil in perfiles)
            {
                Perfiles.Add(perfil);
            }

            if (Perfiles.Count == 0)
            {
                MostrarMensaje("No hay perfiles registrados.");
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

    private async void OnPerfilTapped(object? sender, TappedEventArgs e)
    {
        string perfil = e.Parameter?.ToString() ?? string.Empty;
        await Shell.Current.GoToAsync($"enconstruccion?modulo={Uri.EscapeDataString(perfil)}");
    }
}