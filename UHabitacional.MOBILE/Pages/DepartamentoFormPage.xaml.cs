using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

[QueryProperty(nameof(IdParam), "id")]
[QueryProperty(nameof(EdificioIdParam), "edificioId")]
public partial class DepartamentoFormPage : ContentPage
{
    private readonly DepartamentosService _departamentosService = new();
    private readonly EdificiosService _edificiosService = new();

    private int _idDepartamento;    // 0 = crear, > 0 = editar
    private int _edificioIdInicial; // pre-selección cuando se viene desde filtro
    private bool _inicializado;
    private bool _guardando;

    private List<Edificio> _edificios = [];

    public string IdParam
    {
        set => _idDepartamento = int.TryParse(value, out int id) ? id : 0;
    }

    public string EdificioIdParam
    {
        set => _edificioIdInicial = int.TryParse(value, out int id) ? id : 0;
    }

    private bool EsEdicion => _idDepartamento > 0;

    public DepartamentoFormPage()
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

        TituloLabel.Text = EsEdicion ? "Editar departamento" : "Crear departamento";

        await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        OcultarMensaje();
        MostrarCargando(true);

        try
        {
            _edificios = await _edificiosService.ObtenerEdificiosAsync();
            EdificioPicker.ItemsSource = _edificios;

            if (EsEdicion)
            {
                await CargarDepartamentoAsync();
            }
            else if (_edificioIdInicial > 0)
            {
                // Pre-seleccionar edificio si viene desde filtro
                Edificio? preseleccion = _edificios.FirstOrDefault(e => e.IdEdificio == _edificioIdInicial);
                if (preseleccion is not null)
                {
                    EdificioPicker.SelectedItem = preseleccion;
                }
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

    private async Task CargarDepartamentoAsync()
    {
        Departamento departamento = await _departamentosService.ObtenerDepartamentoPorIdAsync(_idDepartamento);

        Edificio? edificio = _edificios.FirstOrDefault(e => e.IdEdificio == departamento.IdEdificio);
        if (edificio is not null)
        {
            EdificioPicker.SelectedItem = edificio;
        }

        NumeroEntry.Text = departamento.NumeroDepartamento;
        PisoEntry.Text = departamento.Piso.ToString();
    }

    private void OnEdificioSeleccionado(object? sender, EventArgs e)
    {
        if (EdificioPicker.SelectedItem is Edificio edificio)
        {
            PisoLabel.Text = $"Piso (máx. {edificio.NumeroPisos})";
        }
        else
        {
            PisoLabel.Text = "Piso";
        }
    }

    private async void OnGuardarClicked(object? sender, EventArgs e)
    {
        if (_guardando)
        {
            return;
        }

        OcultarMensaje();

        if (EdificioPicker.SelectedItem is not Edificio edificioSeleccionado)
        {
            MostrarMensaje("Debes seleccionar un edificio.");
            return;
        }

        string numero = NumeroEntry.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(numero))
        {
            MostrarMensaje("El número de departamento es obligatorio.");
            return;
        }

        if (!int.TryParse(PisoEntry.Text, out int piso) || piso < 1)
        {
            MostrarMensaje("El piso debe ser un número entero mayor o igual a 1.");
            return;
        }

        if (piso > edificioSeleccionado.NumeroPisos)
        {
            MostrarMensaje($"El piso no puede ser mayor que el máximo del edificio ({edificioSeleccionado.NumeroPisos}).");
            return;
        }

        _guardando = true;
        GuardarButton.IsEnabled = false;
        MostrarCargando(true);

        try
        {
            if (EsEdicion)
            {
                var request = new DepartamentoUpdateRequest
                {
                    IdEdificio = edificioSeleccionado.IdEdificio,
                    NumeroDepartamento = numero,
                    Piso = piso
                };
                await _departamentosService.ActualizarDepartamentoAsync(_idDepartamento, request);
            }
            else
            {
                var request = new DepartamentoCreateRequest
                {
                    IdEdificio = edificioSeleccionado.IdEdificio,
                    NumeroDepartamento = numero,
                    Piso = piso
                };
                await _departamentosService.CrearDepartamentoAsync(request);
            }

            await Shell.Current.GoToAsync("..");
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
