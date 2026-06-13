using UHabitacional.MOBILE.Pages;

namespace UHabitacional.MOBILE;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Registramos las rutas a las que navegamos desde el código.
		Routing.RegisterRoute("home", typeof(HomePage));
		Routing.RegisterRoute("catalogos", typeof(CatalogosPage));
		Routing.RegisterRoute("perfiles", typeof(PerfilesPage));
		Routing.RegisterRoute("tiposidentificacion", typeof(TiposIdentificacionPage));
		Routing.RegisterRoute("vigilantes", typeof(VigilantesPage));
		Routing.RegisterRoute("enconstruccion", typeof(EnConstruccionPage));
	}
}
