using Serilog;
using Uno.Wasm.Bootstrap.Server;

try
{
	Log.Logger = new LoggerConfiguration()
			.WriteTo.Console()
			.WriteTo.File(Path.Combine("App_Data", "Logs", "log.txt"))
			.CreateLogger();
	var builder = WebApplication.CreateBuilder(args);
	builder.Host.UseSerilog();

	// Add services to the container.

	builder.Services.AddControllers();
	builder.Services.Configure<RouteOptions>(options =>
		options.LowercaseUrls = true);

	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen(c =>
	{
		// Include XML comments for all included assemblies
		Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml")
			.Where(x => x.Contains("Demo")
				&& File.Exists(Path.Combine(
					AppContext.BaseDirectory,
					$"{Path.GetFileNameWithoutExtension(x)}.dll")))
			.ToList()
			.ForEach(path => c.IncludeXmlComments(path));
	});

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();

	app.UseAuthorization();

	app.UseUnoFrameworkFiles();
	app.MapFallbackToFile("index.html");

	app.MapControllers();
	app.UseStaticFiles();

	await app.RunAsync();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Application terminated unexpectedly");
	if (System.Diagnostics.Debugger.IsAttached)
	{
		System.Diagnostics.Debugger.Break();
	}
}
finally
{
	Log.CloseAndFlush();
}
