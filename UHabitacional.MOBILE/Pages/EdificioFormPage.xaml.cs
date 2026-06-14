using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

[QueryProperty(nameof(IdParam), "id")]
public partial class EdificioFormPage : ContentPage
{
    private readonly EdificiosService _edificiosService = new();
    private readonly DepartamentosService _departamentosService = new();

    private int _idEdificio;          // 0 = crear, > 0 = editar
    private int _pisosOriginal;
    private int _deptosOriginal;
    private bool _inicializado;
    private bool _guardando;

    public string IdParam
    {
        set => _idEdificio = int.TryParse(value, out int id) ? id : 0;
    }

    private bool EsEdicion => _idEdificio > 0;

    public EdificioFormPage()
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

        TituloLabel.Text = EsEdicion ? "Editar edificio" : "Crear edificio";

        if (EsEdicion)
        {
            await CargarEdificioAsync();
        }
    }

    private async Task CargarEdificioAsync()
    {
        OcultarMensaje();
        MostrarCargando(true);

        try
        {
            Edificio edificio = await _edificiosService.ObtenerEdificioPorIdAsync(_idEdificio);

            NombreEntry.Text = edificio.Nombre;
            DescripcionEditor.Text = edificio.Descripcion;
            NumeroPisosEntry.Text = edificio.NumeroPisos.ToString();
            TotalDeptosEntry.Text = edificio.TotalDeptos.ToString();

            _pisosOriginal = edificio.NumeroPisos;
            _deptosOriginal = edificio.TotalDeptos;
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

    private async void OnGuardarClicked(object? sender, EventArgs e)
    {
        if (_guardando)
        {
            return;
        }
        OcultarMensaje();

        string nombre = NombreEntry.Text?.Trim() ?? string.Empty;
        string descripcion = DescripcionEditor.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(nombre))
        {
            MostrarMensaje("El nombre es obligatorio.");
            return;
        }

        if (!int.TryParse(NumeroPisosEntry.Text, out int numeroPisos) || numeroPisos < 1)
        {
            MostrarMensaje("El número de pisos debe ser un entero mayor o igual a 1.");
            return;
        }

        if (!int.TryParse(TotalDeptosEntry.Text, out int totalDeptos) || totalDeptos < 1)
        {
            MostrarMensaje("El total de departamentos debe ser un entero mayor o igual a 1.");
            return;
        }

        var request = new EdificioRequest
        {
            Nombre = nombre,
            Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion,
            NumeroPisos = numeroPisos,
            TotalDeptos = totalDeptos
        };

        _guardando = true;
        GuardarButton.IsEnabled = false;
        MostrarCargando(true);

        try
        {
            if (EsEdicion)
            {
                if (!await ValidacionesNegocioOkAsync(numeroPisos, totalDeptos))
                {
                    return; // el mensaje ya se mostró
                }

                await _edificiosService.ActualizarEdificioAsync(_idEdificio, request);
            }
            else
            {
                await _edificiosService.CrearEdificioAsync(request);
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

    private async Task<bool> ValidacionesNegocioOkAsync(int numeroPisos, int totalDeptos)
    {
        bool cambioPisos = numeroPisos != _pisosOriginal;
        bool cambioDeptos = totalDeptos != _deptosOriginal;

        if (!cambioPisos && !cambioDeptos)
        {
            return true;
        }

        List<Departamento> departamentos = await _departamentosService.ObtenerDepartamentosAsync();
        List<Departamento> relacionados = departamentos
            .Where(d => d.IdEdificio == _idEdificio)
            .ToList();

        if (cambioDeptos && totalDeptos < relacionados.Count)
        {
            MostrarMensaje($"No puedes asignar menos departamentos ({totalDeptos}) " +
                           $"de los que ya existen ({relacionados.Count}).");
            return false;
        }

        if (cambioPisos)
        {
            int pisoMax = relacionados.Count > 0 ? relacionados.Max(d => d.Piso) : 0;
            if (numeroPisos < pisoMax)
            {
                MostrarMensaje($"No puedes asignar menos pisos ({numeroPisos}) " +
                               $"que el piso más alto ocupado ({pisoMax}).");
                return false;
            }
        }

        return true;
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
