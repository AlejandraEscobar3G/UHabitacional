using UHabitacional.MOBILE.Models;
using UHabitacional.MOBILE.Services;

namespace UHabitacional.MOBILE.Pages;

public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService = new();

    public LoginPage()
    {
        InitializeComponent();
    }

    // Se ejecuta cuando el usuario toca el botón "Iniciar sesión".
    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        string email = EmailEntry.Text?.Trim() ?? string.Empty;
        string password = PasswordEntry.Text ?? string.Empty;

        // Validación sencilla antes de llamar a la API.
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            MostrarError("Escribe tu correo y tu contraseña.");
            return;
        }

        MostrarCargando(true);

        try
        {
            LoginResponse respuesta = await _authService.LoginAsync(email, password);

            // Guardamos los datos del usuario para usarlos en las demás pantallas.
            SessionService.GuardarSesion(respuesta);

            // Entramos al Home.
            await Shell.Current.GoToAsync("home");
        }
        catch (Exception ex)
        {
            MostrarError(ex.Message);
        }
        finally
        {
            MostrarCargando(false);
        }
    }

    private void MostrarError(string mensaje)
    {
        ErrorLabel.Text = mensaje;
        ErrorLabel.IsVisible = true;
    }

    private void MostrarCargando(bool cargando)
    {
        LoadingIndicator.IsVisible = cargando;
        LoadingIndicator.IsRunning = cargando;
        LoginButton.IsEnabled = !cargando;
    }
}
