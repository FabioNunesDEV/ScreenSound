﻿using ScreenSound.WEB.bootstrap.Requests;
using ScreenSound.WEB.bootstrap.Responses;
using System.Net.Http.Json;
using System.Reflection.Metadata;

namespace ScreenSound.WEB.bootstrap.Services;

public class ArtistasAPI
{
    private readonly HttpClient _httpClient;
    public ArtistasAPI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ICollection<ArtistaBase64Response>?> GetArtistasAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<ArtistaBase64Response>>("artistas");
    }

    public async Task<ArtistaBase64Response?> GetArtistaPorIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ArtistaBase64Response>($"artistas/id/{id}");
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