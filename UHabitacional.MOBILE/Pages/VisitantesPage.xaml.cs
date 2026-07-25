using System.Collections.ObjectModel;
using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

public partial class VisitantesPage : ContentPage
{
    private readonly VisitantesService _visitantesService = new();

    public ObservableCollection<Visitante> Visitantes { get; } = new();

    public VisitantesPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarVisitantesAsync();
    }

    private async Task CargarVisitantesAsync()
    {
        MensajeLabel.IsVisible = false;
        MostrarCargando(true);

        try
        {
            List<Visitante> visitantes = await _visitantesService.ObtenerVisitantesPropiosAsync();

            Visitantes.Clear();
            foreach (Visitante v in visitantes)
            {
                Visitantes.Add(v);
            }

            if (Visitantes.Count == 0)
            {
                MostrarMensaje("No tienes visitantes registrados aún.");
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

    private async void OnVisitanteTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is null)
        {
            return;
        }

        int id = Convert.ToInt32(e.Parameter);
        await Shell.Current.GoToAsync($"visitantes/form?id={id}");
    }

    private async void OnNuevoVisitanteTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("visitantes/form");
    }
}
