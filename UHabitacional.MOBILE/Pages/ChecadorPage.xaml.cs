using System.Collections.ObjectModel;
using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

public partial class ChecadorPage : ContentPage
{
    private readonly ChecadorService _checadorService = new();

    private System.Timers.Timer? _reloj;
    private TurnoVigilante? _turnoAbierto;
    private bool _procesando;

    public ObservableCollection<TurnoVigilante> Turnos { get; } = new();

    public ChecadorPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        IniciarReloj();
        await CargarEstadoAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        DetenerReloj();
    }

    // ── Reloj ──────────────────────────────────────────────────────────────

    private void IniciarReloj()
    {
        _reloj = new System.Timers.Timer(1000);
        _reloj.Elapsed += (_, _) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DateTime ahora = DateTime.Now;
                RelojLabel.Text = ahora.ToString("HH:mm:ss");
                FechaLabel.Text = ahora.ToString("dddd, dd 'de' MMMM 'de' yyyy",
                    new System.Globalization.CultureInfo("es-MX"));
            });
        };
        _reloj.AutoReset = true;
        _reloj.Start();

        // Primer tick inmediato
        DateTime now = DateTime.Now;
        RelojLabel.Text = now.ToString("HH:mm:ss");
        FechaLabel.Text = now.ToString("dddd, dd 'de' MMMM 'de' yyyy",
            new System.Globalization.CultureInfo("es-MX"));
    }

    private void DetenerReloj()
    {
        _reloj?.Stop();
        _reloj?.Dispose();
        _reloj = null;
    }

    // ── Carga de datos ────────────────────────────────────────────────────

    private async Task CargarEstadoAsync()
    {
        OcultarMensaje();
        MostrarCargando(true);
        AccionButton.IsEnabled = false;

        try
        {
            List<TurnoVigilante> turnos = await _checadorService.ObtenerTurnosPropiosAsync();

            Turnos.Clear();
            foreach (TurnoVigilante t in turnos)
            {
                Turnos.Add(t);
            }

            HistorialVacioLabel.IsVisible = Turnos.Count == 0;

            // El turno abierto es el más reciente sin salida
            _turnoAbierto = turnos.FirstOrDefault(t => t.EstaAbierto);
            ActualizarUI();
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message);
        }
        finally
        {
            MostrarCargando(false);
            AccionButton.IsEnabled = true;
        }
    }

    private void ActualizarUI()
    {
        if (_turnoAbierto is not null)
        {
            // EN TURNO
            EstadoBadge.BackgroundColor = Color.FromArgb("#E8F5E9");
            EstadoLabel.Text = "● EN TURNO";
            EstadoLabel.TextColor = Color.FromArgb("#2D6A4F");

            InfoTurnoLabel.Text = $"Turno iniciado a las {_turnoAbierto.EntradaTexto}";
            InfoTurnoLabel.IsVisible = true;

            AccionButton.Text = "Registrar salida";
            AccionButton.BackgroundColor = Color.FromArgb("#CB6843"); // Accent
        }
        else
        {
            // FUERA DE TURNO
            EstadoBadge.BackgroundColor = Color.FromArgb("#F5F5F5");
            EstadoLabel.Text = "○ FUERA DE TURNO";
            EstadoLabel.TextColor = Color.FromArgb("#6E6E6E");

            InfoTurnoLabel.IsVisible = false;

            AccionButton.Text = "Registrar entrada";
            AccionButton.BackgroundColor = Color.FromArgb("#2D4A3E"); // Primary
        }
    }

    // ── Acción principal ──────────────────────────────────────────────────

    private async void OnAccionClicked(object? sender, EventArgs e)
    {
        if (_procesando)
        {
            return;
        }

        _procesando = true;
        AccionButton.IsEnabled = false;
        OcultarMensaje();
        MostrarCargando(true);

        try
        {
            if (_turnoAbierto is null)
            {
                await _checadorService.RegistrarEntradaAsync();
            }
            else
            {
                await _checadorService.RegistrarSalidaAsync(_turnoAbierto.IdBitacoraVigilante);
            }

            // Recargar estado completo
            await CargarEstadoAsync();
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message);
            MostrarCargando(false);
            AccionButton.IsEnabled = true;
        }
        finally
        {
            _procesando = false;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private async void OnRegresarTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
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

    private void OcultarMensaje()
    {
        MensajeLabel.IsVisible = false;
    }
}
