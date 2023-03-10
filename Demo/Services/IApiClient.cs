using Demo.DataContracts;

namespace Demo.Services;

[Headers("Content-Type: application/json")]
public interface IApiClient
{
	[Get("/api/weatherforecast")]
	Task<ApiResponse<IEnumerable<WeatherForecast>>> GetWeather(CancellationToken cancellationToken = default);
}
