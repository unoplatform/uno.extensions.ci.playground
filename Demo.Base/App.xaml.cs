namespace Demo;

public sealed partial class App : global::Microsoft.UI.Xaml.Application
{
	private Window? _window;
	private IHost? _host;

	public App()
	{
		this.InitializeComponent();
	}

	/// <summary>
	/// Invoked when the application is launched normally by the end user.  Other entry points
	/// will be used such as when the application is launched to open a specific file.
	/// </summary>
	/// <param name="args">Details about the launch request and process.</param>
	protected async override void OnLaunched(LaunchActivatedEventArgs args)
	{
		var builder = this.CreateBuilder(args)
			// Add navigation support for toolkit controls such as TabBar and NavigationView
			.UseToolkitNavigation()
			.Configure(host => host
#if DEBUG
				// Switch to Development environment when running in DEBUG
				.UseEnvironment(Environments.Development)
#endif
				.UseLogging(configure: (context, logBuilder) =>
				{
					// Configure log levels for different categories of logging
					logBuilder.SetMinimumLevel(
						context.HostingEnvironment.IsDevelopment() ?
							LogLevel.Information :
							LogLevel.Warning);
				}, enableUnoLogging: true)
				.UseSerilog(consoleLoggingEnabled: true, fileLoggingEnabled: true)
				.UseConfiguration(configure: configBuilder =>
					configBuilder
						.EmbeddedSource<AppConfig>()
						.Section<AppConfig>()
				)
				// Enable localization (see appsettings.json for supported languages)
				.UseLocalization()
				// Register Json serializers (ISerializer and ISerializer)
				.UseSerialization()
				.ConfigureServices((context, services) => {
					// Register HttpClient
					services
#if DEBUG
						// DelegatingHandler will be automatically injected into Refit Client
						.AddTransient<DelegatingHandler, DebugHttpHandler>()
#endif
						.AddRefitClient<IApiClient>(context);

					// TODO: Register your services
					//services.AddSingleton<IMyService, MyService>();
				})
				.UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes)
			);
		_window = builder.Window;

		_host = await builder.NavigateAsync<Shell>();
	}

	private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
	{
		views.Register(
			new ViewMap(ViewModel: typeof(ShellModel)),
			new ViewMap<MainPage, MainModel>(),
			new DataViewMap<SecondPage, SecondModel, Entity>()
		);

		routes.Register(
			new RouteMap("", View: views.FindByViewModel<ShellModel>(),
				Nested: new RouteMap[]
				{
					new RouteMap("Main", View: views.FindByViewModel<MainModel>()),
					new RouteMap("Second", View: views.FindByViewModel<SecondModel>()),
				}
			)
		);
	}
}
