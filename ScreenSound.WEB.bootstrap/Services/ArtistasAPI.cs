using ScreenSound.Shared.Requests;
using ScreenSound.Shared.Response;
using System.Net.Http.Json;


namespace ScreenSound.WEB.bootstrap.Services;

public class ArtistasAPI
{
    private readonly HttpClient _httpClient;
    public ArtistasAPI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ICollection<ArtistaResponse>?> GetArtistasAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<ArtistaResponse>>("artistas");
    }

    public async Task<ArtistaResponse?> GetArtistaPorIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ArtistaResponse>($"artistas/id/{id}");
    }

    public async Task AddArtistaAsync(ArtistaRequest artista)
    {
        await _httpClient.PostAsJsonAsync("artistas", artista);
    }

    public async Task DeleteArtistaAsync(int id)
    {
        await _httpClient.DeleteAsync($"artistas/{id}");
    }

    public async Task<ArtistaResponse?> GetArtistaPorNomeAsync(string nome)
    {
        return await _httpClient.GetFromJsonAsync<ArtistaResponse>($"artistas/{nome}");
    }
    public async Task UpdateArtistaAsync(ArtistaRequestEdit artista)
    {
        await _httpClient.PutAsJsonAsync($"artistas", artista);
    }

}