namespace stand_alone_api_client_with_imemcache.Services
{
    public interface IOpenWeatherService
    {
        Task<dynamic> GetWeatherAsync(string city);
    }
}
