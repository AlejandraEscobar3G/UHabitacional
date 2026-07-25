using System.Collections.ObjectModel;
using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

public partial class InquilinosPage : ContentPage
{
    private readonly InquilinosService _inquilinosService = new();

    public ObservableCollection<Inquilino> Inquilinos { get; } = new();

    public InquilinosPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarInquilinosAsync();
    }

    private async Task CargarInquilinosAsync()
    {
        MensajeLabel.IsVisible = false;
        MostrarCargando(true);

        try
        {
            List<Inquilino> inquilinos = await _inquilinosService.ObtenerInquilinosAsync();

            Inquilinos.Clear();
            foreach (Inquilino inquilino in inquilinos)
            {
                Inquilinos.Add(inquilino);
            }

            if (Inquilinos.Count == 0)
            {
                MostrarMensaje("No hay inquilinos registrados.");
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

    private async void OnInquilinoTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is null)
        {
            return;
        }

        int id = Convert.ToInt32(e.Parameter);
        await Shell.Current.GoToAsync($"inquilinos/form?id={id}");
    }

    private async void OnNuevoInquilinoTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("inquilinos/form");
    }
}
