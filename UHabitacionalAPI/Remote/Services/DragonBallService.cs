using System.Text.Json;
using UHabitacionalAPI.Remote.DTOs;
using UHabitacionalAPI.Remote.Interfaces;

namespace UHabitacionalAPI.Remote.Services
{
    public class DragonBallService : IDragonBallService
    {
        private readonly HttpClient _httpClient;
        public DragonBallService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DragonBallCharactersResponse?> GetAllCharactersAsync(int? page = null, int? limit = null)
        {
            string url = "https://dragonball-api.com/api/characters";

            if (page.HasValue && limit.HasValue)
            {
                url += $"?page={page.Value}&limit={limit.Value}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DragonBallCharactersResponse>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }

        public async Task<DragonBallCharacterResponse?> GetCharacterAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://dragonball-api.com/api/characters/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DragonBallCharacterResponse>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }
    }
}
