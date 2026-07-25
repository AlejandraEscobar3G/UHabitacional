using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

[QueryProperty(nameof(IdParam), "id")]
public partial class InquilinoFormPage : ContentPage
{
    private readonly InquilinosService _inquilinosService = new();
    private readonly DepartamentosService _departamentosService = new();

    private int _idInquilino;       // 0 = crear, > 0 = editar
    private bool _inicializado;
    private bool _guardando;
    private DateTime? _fechaFinOriginal;

    private List<Usuario> _usuarios = [];
    private List<Departamento> _departamentos = [];

    public string IdParam
    {
        set => _idInquilino = int.TryParse(value, out int id) ? id : 0;
    }

    private bool EsEdicion => _idInquilino > 0;

    public InquilinoFormPage()
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

        TituloLabel.Text = EsEdicion ? "Editar inquilino" : "Crear inquilino";
        FechaInicioPicker.Date = DateTime.Today;
        FechaFinPicker.Date = DateTime.Today;

        if (EsEdicion)
        {
            UsuarioSection.IsVisible = false;
            NombreUsuarioSection.IsVisible = true;
            FechaFinSection.IsVisible = true;
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
            // Cargar departamentos (ambos modos los necesitan)
            _departamentos = await _departamentosService.ObtenerDepartamentosAsync();
            DepartamentoPicker.ItemsSource = _departamentos;

            if (!EsEdicion)
            {
                // Modo crear: cargar usuarios de tipo Inquilino
                _usuarios = await _inquilinosService.ObtenerUsuariosInquilinoAsync();
                UsuarioPicker.ItemsSource = _usuarios;

                if (_usuarios.Count == 0)
                {
                    MostrarMensaje("No hay usuarios de tipo Inquilino disponibles.");
                }
            }
            else
            {
                // Modo editar: cargar datos del inquilino
                await CargarInquilinoAsync();
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

    private async Task CargarInquilinoAsync()
    {
        Inquilino inquilino = await _inquilinosService.ObtenerInquilinoPorIdAsync(_idInquilino);

        NombreUsuarioLabel.Text = inquilino.NombreCompleto;

        // Seleccionar el departamento actual en el Picker
        Departamento? deptoActual = _departamentos.FirstOrDefault(d => d.IdDepartamento == inquilino.IdDepartamento);
        if (deptoActual is not null)
        {
            DepartamentoPicker.SelectedItem = deptoActual;
        }

        FechaInicioPicker.Date = inquilino.FechaInicio;

        // Configurar fecha de salida
        _fechaFinOriginal = inquilino.FechaFin;
        if (inquilino.FechaFin.HasValue)
        {
            FechaFinSwitch.IsToggled = true;
            FechaFinPickerContainer.IsVisible = true;
            FechaFinPicker.Date = inquilino.FechaFin.Value;
        }
        else
        {
            FechaFinSwitch.IsToggled = false;
            FechaFinPickerContainer.IsVisible = false;
        }
    }

    private void OnFechaFinSwitchToggled(object? sender, ToggledEventArgs e)
    {
        FechaFinPickerContainer.IsVisible = e.Value;
    }

    private async void OnGuardarClicked(object? sender, EventArgs e)
    {
        if (_guardando)
        {
            return;
        }

        OcultarMensaje();

        if (!EsEdicion)
        {
            await GuardarCrearAsync();
        }
        else
        {
            await GuardarEditarAsync();
        }
    }

    private async Task GuardarCrearAsync()
    {
        if (UsuarioPicker.SelectedItem is not Usuario usuarioSeleccionado)
        {
            MostrarMensaje("Debes seleccionar un usuario inquilino.");
            return;
        }

        if (DepartamentoPicker.SelectedItem is not Departamento deptoSeleccionado)
        {
            MostrarMensaje("Debes seleccionar un departamento.");
            return;
        }

        var request = new InquilinoCreateRequest
        {
            IdUsuario = usuarioSeleccionado.IdUsuario,
            IdDepartamento = deptoSeleccionado.IdDepartamento,
            FechaInicio = FechaInicioPicker.Date
        };

        _guardando = true;
        GuardarButton.IsEnabled = false;
        MostrarCargando(true);

        try
        {
            await _inquilinosService.CrearInquilinoAsync(request);
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

    private async Task GuardarEditarAsync()
    {
        if (DepartamentoPicker.SelectedItem is not Departamento deptoSeleccionado)
        {
            MostrarMensaje("Debes seleccionar un departamento.");
            return;
        }

        _guardando = true;
        GuardarButton.IsEnabled = false;
        MostrarCargando(true);

        try
        {
            // Actualizar departamento y fecha de inicio
            var updateRequest = new InquilinoUpdateRequest
            {
                IdDepartamento = deptoSeleccionado.IdDepartamento,
                FechaInicio = FechaInicioPicker.Date
            };
            await _inquilinosService.ActualizarInquilinoAsync(_idInquilino, updateRequest);

            // Actualizar fecha de salida si cambió
            DateTime? nuevaFechaFin = FechaFinSwitch.IsToggled ? FechaFinPicker.Date : null;
            bool fechaFinCambio = nuevaFechaFin != _fechaFinOriginal;

            if (fechaFinCambio)
            {
                await _inquilinosService.ActualizarFechaFinAsync(_idInquilino, nuevaFechaFin);
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

    private async void OnEliminarClicked(object? sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert(
            "Eliminar inquilino",
            "¿Estás seguro de que deseas eliminar este inquilino? Esta acción no se puede deshacer.",
            "Eliminar",
            "Cancelar");

        if (!confirmar)
        {
            return;
        }

        MostrarCargando(true);
        EliminarButton.IsEnabled = false;

        try
        {
            await _inquilinosService.EliminarInquilinoAsync(_idInquilino);
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
