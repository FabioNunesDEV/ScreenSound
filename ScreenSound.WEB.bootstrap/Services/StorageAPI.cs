using System.Net.Http.Json;

namespace ScreenSound.WEB.bootstrap.Services;

public class StorageAPI
{
    private readonly HttpClient _httpClient;
    public StorageAPI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> UploadFileAsync(string base64Image, string fileName)
    {
        var request = new
        {
            Base64Imagem = base64Image,
            NomeArquivo = fileName
        };
        var response = await _httpClient.PostAsJsonAsync("storage/upload", request);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> DownloadFileAsync(string fileName)
    {
        var response = await _httpClient.GetAsync($"storage/download/{fileName}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new FileNotFoundException($"File {fileName} not found.");
        }
        else
        {
            response.EnsureSuccessStatusCode();
            return string.Empty; // This line should not be reached
        }
    }
}
