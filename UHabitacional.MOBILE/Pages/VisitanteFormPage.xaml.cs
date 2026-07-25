using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

[QueryProperty(nameof(IdParam), "id")]
public partial class VisitanteFormPage : ContentPage
{
    private readonly VisitantesService _visitantesService = new();
    private readonly TiposIdentificacionService _identificacionService = new();

    private int _idVisitante;      // 0 = crear, > 0 = editar
    private bool _inicializado;
    private bool _guardando;
    private string _codigoActual = string.Empty;

    private List<Identificacion> _identificaciones = [];

    public string IdParam
    {
        set => _idVisitante = int.TryParse(value, out int id) ? id : 0;
    }

    private bool EsEdicion => _idVisitante > 0;

    public VisitanteFormPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_inicializado)
        {
            return;
        }
        _inicializado = true;

        TituloLabel.Text = EsEdicion ? "Editar visitante" : "Registrar visitante";

        if (EsEdicion)
        {
            InfoCodigoBox.IsVisible = false;
            CodigoActualSection.IsVisible = true;
            EliminarButton.IsVisible = true;
        }

        await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        OcultarMensaje();
        MostrarCargando(true);

        try
        {
            _identificaciones = await _identificacionService.ObtenerTiposIdentificacionAsync();
            IdentificacionPicker.ItemsSource = _identificaciones;

            if (EsEdicion)
            {
                await CargarVisitanteAsync();
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

    private async Task CargarVisitanteAsync()
    {
        Visitante visitante = await _visitantesService.ObtenerVisitantePorIdAsync(_idVisitante);

        NombreEntry.Text = visitante.NombreVisitante;
        NumeroIdEntry.Text = visitante.NumeroIdentificacion;
        ObservacionesEditor.Text = visitante.Observaciones;

        Identificacion? idActual = _identificaciones.FirstOrDefault(i => i.IdIdentificacion == visitante.IdIdentificacion);
        if (idActual is not null)
        {
            IdentificacionPicker.SelectedItem = idActual;
        }

        _codigoActual = visitante.CodigoVisita;
        CodigoLabel.Text = visitante.CodigoVisita;

        // Si el visitante ya llegó, solo se puede ver el código (no editar)
        if (visitante.FechaHoraLlegada.HasValue)
        {
            NombreEntry.IsEnabled = false;
            IdentificacionPicker.IsEnabled = false;
            NumeroIdEntry.IsEnabled = false;
            ObservacionesEditor.IsEnabled = false;
            GuardarButton.IsVisible = false;
            EliminarButton.IsVisible = false;
            MostrarMensaje("Este visitante ya registró su llegada; no se pueden modificar los datos.");
        }
    }

    private async void OnGuardarClicked(object? sender, EventArgs e)
    {
        if (_guardando)
        {
            return;
        }

        OcultarMensaje();

        string nombre = NombreEntry.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(nombre))
        {
            MostrarMensaje("El nombre del visitante es obligatorio.");
            return;
        }

        if (IdentificacionPicker.SelectedItem is not Identificacion idSeleccionada)
        {
            MostrarMensaje("Debes seleccionar el tipo de identificación.");
            return;
        }

        string numeroId = NumeroIdEntry.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(numeroId))
        {
            MostrarMensaje("El número de identificación es obligatorio.");
            return;
        }

        string? observaciones = ObservacionesEditor.Text?.Trim();
        if (string.IsNullOrWhiteSpace(observaciones))
        {
            observaciones = null;
        }

        _guardando = true;
        GuardarButton.IsEnabled = false;
        MostrarCargando(true);

        try
        {
            if (!EsEdicion)
            {
                var request = new VisitanteCreateRequest
                {
                    NombreVisitante = nombre,
                    IdIdentificacion = idSeleccionada.IdIdentificacion,
                    NumeroIdentificacion = numeroId,
                    Observaciones = observaciones
                };

                Visitante creado = await _visitantesService.CrearVisitanteAsync(request);

                // Navegar a pantalla de código generado (apilada sobre el formulario)
                await Shell.Current.GoToAsync(
                    $"visitantes/codigo?codigo={Uri.EscapeDataString(creado.CodigoVisita)}&nombre={Uri.EscapeDataString(nombre)}");
            }
            else
            {
                var request = new VisitanteUpdateRequest
                {
                    NombreVisitante = nombre,
                    IdIdentificacion = idSeleccionada.IdIdentificacion,
                    NumeroIdentificacion = numeroId,
                    Observaciones = observaciones
                };

                await _visitantesService.ActualizarVisitanteAsync(_idVisitante, request);
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message);
        }
        finally
        {
            _guardando = false;
            GuardarButton.IsEnabled = true;
            MostrarCargando(false);
        }
    }

    private async void OnEliminarClicked(object? sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert(
            "Cancelar visita",
            "¿Estás seguro de que deseas cancelar esta visita? Esta acción no se puede deshacer.",
            "Cancelar visita",
            "No, mantener");

        if (!confirmar)
        {
            return;
        }

        MostrarCargando(true);
        EliminarButton.IsEnabled = false;

        try
        {
            await _visitantesService.EliminarVisitanteAsync(_idVisitante);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message);
        }
        finally
        {
            MostrarCargando(false);
            EliminarButton.IsEnabled = true;
        }
    }

    private async void OnCopiarCodigoTapped(object? sender, TappedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(_codigoActual))
        {
            await Clipboard.SetTextAsync(_codigoActual);
            await DisplayAlert("Copiado", $"Código {_codigoActual} copiado al portapapeles.", "OK");
        }
    }

    private async void OnCancelarClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

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
