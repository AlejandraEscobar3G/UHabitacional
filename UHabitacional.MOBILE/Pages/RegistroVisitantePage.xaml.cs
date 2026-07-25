using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

public partial class RegistroVisitantePage : ContentPage
{
    private readonly RegistroVisitanteService _service = new();

    private Visitante? _visitanteActual;
    private bool _procesando;

    public RegistroVisitantePage()
    {
        InitializeComponent();
    }

    // Habilitar botón solo cuando hay exactamente 6 caracteres
    private void OnCodigoTextChanged(object? sender, TextChangedEventArgs e)
    {
        string texto = (e.NewTextValue ?? string.Empty).Trim();
        ValidarButton.IsEnabled = texto.Length == 6;

        // Si el usuario borró el código, ocultar resultado anterior
        if (texto.Length == 0)
        {
            OcultarResultado();
        }
    }

    private async void OnValidarClicked(object? sender, EventArgs e)
    {
        string codigo = (CodigoEntry.Text ?? string.Empty).Trim().ToUpper();
        if (codigo.Length != 6)
        {
            return;
        }

        await BuscarCodigoAsync(codigo);
    }

    private async Task BuscarCodigoAsync(string codigo)
    {
        OcultarResultado();
        OcultarError();
        MostrarCargando(true);
        ValidarButton.IsEnabled = false;

        try
        {
            Visitante? visitante = await _service.BuscarPorCodigoAsync(codigo);

            if (visitante is null)
            {
                MostrarError("El código no existe o ya no está activo. Verifica con el visitante.");
            }
            else
            {
                _visitanteActual = visitante;
                MostrarResultado(visitante);
            }
        }
        catch (Exception ex)
        {
            MostrarError(ex.Message);
        }
        finally
        {
            MostrarCargando(false);
            ValidarButton.IsEnabled = CodigoEntry.Text?.Trim().Length == 6;
        }
    }

    private void MostrarResultado(Visitante v)
    {
        NombreVisitanteLabel.Text = v.NombreVisitante;
        IdentificacionLabel.Text = v.InfoIdentificacion;
        InquilinoLabel.Text = v.NombreInquilino ?? "—";
        ObservacionesLabel.Text = string.IsNullOrWhiteSpace(v.Observaciones) ? "—" : v.Observaciones;
        CodigoResultadoLabel.Text = v.CodigoVisita;

        bool tieneEntrada = v.FechaHoraLlegada.HasValue;
        bool tieneSalida = v.FechaHoraSalida.HasValue;

        EntradaTituloLabel.IsVisible = tieneEntrada;
        EntradaLabel.IsVisible = tieneEntrada;
        if (tieneEntrada)
        {
            EntradaLabel.Text = v.FechaHoraLlegada!.Value.ToString("dd MMM HH:mm");
        }

        SalidaTituloLabel.IsVisible = tieneSalida;
        SalidaLabel.IsVisible = tieneSalida;
        if (tieneSalida)
        {
            SalidaLabel.Text = v.FechaHoraSalida!.Value.ToString("dd MMM HH:mm");
        }

        // Botones adaptativos según estado
        EntradaButton.IsVisible = !tieneEntrada;
        SalidaButton.IsVisible = tieneEntrada && !tieneSalida;
        CompletadaBadge.IsVisible = tieneEntrada && tieneSalida;

        AccionMensajeLabel.IsVisible = false;
        ResultadoCard.IsVisible = true;
    }

    private void OcultarResultado()
    {
        ResultadoCard.IsVisible = false;
        _visitanteActual = null;
    }

    // ── Registrar entrada ─────────────────────────────────────────────────

    private async void OnEntradaClicked(object? sender, EventArgs e)
    {
        if (_procesando || _visitanteActual is null)
        {
            return;
        }

        _procesando = true;
        EntradaButton.IsEnabled = false;
        MostrarAccionCargando(true);
        OcultarAccionMensaje();

        try
        {
            Visitante actualizado = await _service.RegistrarEntradaAsync(_visitanteActual.IdBitacoraVisitante);
            _visitanteActual = actualizado;
            MostrarResultado(actualizado);
            MostrarAccionMensaje("Entrada registrada correctamente.", esError: false);
        }
        catch (Exception ex)
        {
            MostrarAccionMensaje(ex.Message, esError: true);
            EntradaButton.IsEnabled = true;
        }
        finally
        {
            _procesando = false;
            MostrarAccionCargando(false);
        }
    }

    // ── Registrar salida ──────────────────────────────────────────────────

    private async void OnSalidaClicked(object? sender, EventArgs e)
    {
        if (_procesando || _visitanteActual is null)
        {
            return;
        }

        _procesando = true;
        SalidaButton.IsEnabled = false;
        MostrarAccionCargando(true);
        OcultarAccionMensaje();

        try
        {
            Visitante actualizado = await _service.RegistrarSalidaAsync(_visitanteActual.IdBitacoraVisitante);
            _visitanteActual = actualizado;
            MostrarResultado(actualizado);
            MostrarAccionMensaje("Salida registrada correctamente.", esError: false);
        }
        catch (Exception ex)
        {
            MostrarAccionMensaje(ex.Message, esError: true);
            SalidaButton.IsEnabled = true;
        }
        finally
        {
            _procesando = false;
            MostrarAccionCargando(false);
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

    private void MostrarAccionCargando(bool cargando)
    {
        AccionLoadingIndicator.IsVisible = cargando;
        AccionLoadingIndicator.IsRunning = cargando;
    }

    private void MostrarError(string mensaje)
    {
        ErrorLabel.Text = mensaje;
        ErrorLabel.IsVisible = true;
    }

    private void OcultarError()
    {
        ErrorLabel.IsVisible = false;
    }

    private void MostrarAccionMensaje(string mensaje, bool esError)
    {
        AccionMensajeLabel.Text = mensaje;
        AccionMensajeLabel.TextColor = esError
            ? Color.FromArgb("#C0392B")   // Danger
            : Color.FromArgb("#2D6A4F");  // Success
        AccionMensajeLabel.IsVisible = true;
    }

    private void OcultarAccionMensaje()
    {
        AccionMensajeLabel.IsVisible = false;
    }
}
