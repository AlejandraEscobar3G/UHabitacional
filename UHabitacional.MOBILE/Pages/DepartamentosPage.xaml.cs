using System.Collections.ObjectModel;
using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

public partial class DepartamentosPage : ContentPage
{
    private readonly DepartamentosService _departamentosService = new();
    private readonly EdificiosService _edificiosService = new();

    private List<Departamento> _todosDepartamentos = [];
    private List<Edificio> _edificios = [];

    public ObservableCollection<Departamento> DepartamentosFiltrados { get; } = new();

    public DepartamentosPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        MensajeLabel.IsVisible = false;
        MostrarCargando(true);

        try
        {
            // Cargar edificios y departamentos en paralelo
            Task<List<Edificio>> tarEdificios = _edificiosService.ObtenerEdificiosAsync();
            Task<List<Departamento>> tarDepartamentos = _departamentosService.ObtenerDepartamentosAsync();

            await Task.WhenAll(tarEdificios, tarDepartamentos);

            _edificios = tarEdificios.Result;
            _todosDepartamentos = tarDepartamentos.Result;

            // Poblar el Picker de edificios (con opción "Todos" al inicio)
            var opcionTodos = new Edificio { IdEdificio = 0, Nombre = "Todos los edificios" };
            var listaEdificios = new List<Edificio> { opcionTodos };
            listaEdificios.AddRange(_edificios);
            EdificioPicker.ItemsSource = listaEdificios;
            EdificioPicker.SelectedIndex = 0;

            AplicarFiltro(0);
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

    private void OnEdificioFiltroChanged(object? sender, EventArgs e)
    {
        if (EdificioPicker.SelectedItem is not Edificio seleccionado)
        {
            return;
        }

        AplicarFiltro(seleccionado.IdEdificio);
    }

    private void AplicarFiltro(int idEdificio)
    {
        DepartamentosFiltrados.Clear();

        IEnumerable<Departamento> filtrados = idEdificio == 0
            ? _todosDepartamentos
            : _todosDepartamentos.Where(d => d.IdEdificio == idEdificio);

        foreach (Departamento departamento in filtrados.OrderBy(d => d.Piso).ThenBy(d => d.NumeroDepartamento))
        {
            DepartamentosFiltrados.Add(departamento);
        }

        if (DepartamentosFiltrados.Count == 0)
        {
            MostrarMensaje("No hay departamentos registrados.");
        }
        else
        {
            MensajeLabel.IsVisible = false;
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

    private async void OnDepartamentoTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is null)
        {
            return;
        }

        int id = Convert.ToInt32(e.Parameter);
        await Shell.Current.GoToAsync($"departamentos/form?id={id}");
    }

    private async void OnNuevoDepartamentoTapped(object? sender, TappedEventArgs e)
    {
        // Si hay un edificio filtrado, lo pre-seleccionamos en el formulario
        int idEdificio = 0;
        if (EdificioPicker.SelectedItem is Edificio seleccionado && seleccionado.IdEdificio != 0)
        {
            idEdificio = seleccionado.IdEdificio;
        }

        string ruta = idEdificio > 0
            ? $"departamentos/form?edificioId={idEdificio}"
            : "departamentos/form";

        await Shell.Current.GoToAsync(ruta);
    }
}
