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
		Routing.RegisterRoute("edificios", typeof(EdificiosPage));
		Routing.RegisterRoute("edificios/id", typeof(EdificioFormPage));
		Routing.RegisterRoute("inquilinos", typeof(InquilinosPage));
		Routing.RegisterRoute("inquilinos/form", typeof(InquilinoFormPage));
		Routing.RegisterRoute("departamentos", typeof(DepartamentosPage));
		Routing.RegisterRoute("departamentos/form", typeof(DepartamentoFormPage));
		Routing.RegisterRoute("visitantes", typeof(VisitantesPage));
		Routing.RegisterRoute("visitantes/form", typeof(VisitanteFormPage));
		Routing.RegisterRoute("visitantes/codigo", typeof(VisitanteCodigoPage));
		Routing.RegisterRoute("checador", typeof(ChecadorPage));
		Routing.RegisterRoute("registrovisitante", typeof(RegistroVisitantePage));
		Routing.RegisterRoute("enconstruccion", typeof(EnConstruccionPage));
	}
}
