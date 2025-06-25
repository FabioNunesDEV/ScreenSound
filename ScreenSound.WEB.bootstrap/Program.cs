using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ScreenSound.WEB.bootstrap;
using ScreenSound.WEB.bootstrap.Services;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Carrega o appsettings.json manualmente
using var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var config = await http.GetFromJsonAsync<Dictionary<string, Dictionary<string, string>>>("appsettings.json");
var apiUrl = config?["APIServer"]["Url"] ?? throw new Exception("APIServer:Url não encontrado em appsettings.json");

// Agora registre o HttpClient com a URL correta
builder.Services.AddHttpClient<ArtistasAPI>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Novo serviço para o StorageAPI
builder.Services.AddHttpClient<StorageAPI>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

await builder.Build().RunAsync();
