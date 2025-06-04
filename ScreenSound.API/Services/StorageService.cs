using Microsoft.Extensions.Configuration;

namespace ScreenSound.API.Services;
public class StorageService
{
    private readonly string _storagePath;

    /// <summary>
    /// Construtor que recebe o caminho de armazenamento a partir da configuração appconfig.json.
    /// </summary>
    /// <param name="configuration">Objeto que fornece acesso às configurações da aplicação</param>
    public StorageService(IConfiguration configuration)
    {
        _storagePath = configuration.GetSection("Storage")["Path"]!;
    }

    /// <summary>
    /// Método para salvar uma imagem em formato Base64 no caminho de armazenamento definido.
    /// </summary>
    /// <param name="base64">base64 da imagem passada por parametro.</param>
    /// <param name="extensao">Extensão da imagem passada por parametro.</param>
    /// <param name="nomeArtista">nome do artista ou banda para ser usado no nome do arquivo.</param>
    /// <returns>Retorna o caminho absoluto completo do arquivo com a imagem do artista.</returns>
    public async Task<string> SalvarImagemBase64Async(string base64, string extensao , string nomeArtista)
    {
        // normaliza o nome do artista para evitar caracteres especiais e espaços.
        string nomeNormalizado = Util.NormalizarString(nomeArtista);

        // Verifica se o caminho de armazenamento existe, caso não exista, cria o diretório.
        if (!Directory.Exists(_storagePath))
            Directory.CreateDirectory(_storagePath);

        // cria um guid que será concatenado ao nome do artista para formar o caminho completo do aquivo.
        var guid = Guid.NewGuid();
        var nomeArquivo = $"{guid}_{nomeNormalizado.Trim()}.{extensao}";
        var caminhoCompleto = Path.Combine(_storagePath, nomeArquivo);

        // converte o base64 para byte[] e salva no caminho completo.
        using var ms = new MemoryStream(Convert.FromBase64String(base64));
        using var fs = new FileStream(caminhoCompleto, FileMode.Create);
        await ms.CopyToAsync(fs);

        // Retorna caminho absoluto do arquivo salvo em disco.
        return caminhoCompleto;
    }

    /// <summary>
    /// Método para concatenar o caminho completo do card de um artista ou banda.
    /// </summary>
    /// <param name="nomeCard">Nome do arquivo Card no storage</param>
    /// <returns>Retorna o caminho completo para o Card</returns>
    public string ConcatenarCaminhoCompletoCard (string nomeCard)
    {
        var caminhoCompleto = Path.Combine(_storagePath, nomeCard);

        return caminhoCompleto;
    }

    /// <summary>
    /// Método para ler uma imagem do disco e retornar seu conteúdo em Base64.
    /// </summary>
    /// <param name="caminhoFisico">Caminho físico completo da imagem.</param>
    /// <returns>String Base64 representando a imagem.</returns>
    public async Task<string> ObterImagemBase64Async(string caminhoFisico)
    {
        if (!File.Exists(caminhoFisico))
            throw new FileNotFoundException("Arquivo de imagem não encontrado.", caminhoFisico);

        byte[] bytes;
        await using (var fs = new FileStream(caminhoFisico, FileMode.Open, FileAccess.Read))
        {
            bytes = new byte[fs.Length];
            await fs.ReadAsync(bytes, 0, (int)fs.Length);
        }
        return $"data:image/png;base64,{Convert.ToBase64String(bytes)}" ;
    }
}

