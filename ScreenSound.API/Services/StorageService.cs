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
    /// Método para salvar uma imagem .jpg em formato Base64 no caminho de armazenamento definido.
    /// </summary>
    /// <param name="base64">base64 da imagem .jpg passada por parametro.</param>
    /// <param name="nomeArquivo">Nome do Artista para criar o card.</param>
    public async Task SalvarImagemBase64Async(string base64 , string nomeArquivo)
    {
        this.GarantirDiretorioExiste(_storagePath);

        string caminhoCompleto = Path.Combine(_storagePath, nomeArquivo);

        // converte o base64 para byte[] e salva no caminho completo.
        using var ms = new MemoryStream(Convert.FromBase64String(base64));
        using var fs = new FileStream(caminhoCompleto, FileMode.Create);
        await ms.CopyToAsync(fs);
    }

    /// <summary>
    /// Método para ler uma imagem do disco e retornar seu conteúdo em Base64.
    /// </summary>
    /// <param name="NomeCard">Nome do arquivo do card da artista.</param>
    /// <returns>String Base64 representando a imagem.</returns>
    public async Task<string> LerBase64Async(string NomeCard)
    {
        string caminhoFisico = this.ConcatenarCaminhoCompletoCard(NomeCard);

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

    /// <summary>
    /// Método para concatenar o caminho completo do card de um artista ou banda.
    /// </summary>
    /// <param name="nomeCard">Nome do arquivo Card no storage</param>
    /// <returns>Retorna o caminho completo para o Card</returns>
    private string ConcatenarCaminhoCompletoCard(string nomeCard)
    {
        var caminhoCompleto = Path.Combine(_storagePath, nomeCard);

        return caminhoCompleto;
    }

    /// <summary>
    /// Verifica a existensia de um diretório e o cria se não existir.
    /// </summary>
    /// <param name="caminho">Caminho completo do diretório.</param>
    private void GarantirDiretorioExiste(string caminho)
    {
        if (!Directory.Exists(caminho))
            Directory.CreateDirectory(caminho);
    }

    /// <summary>
    /// Gera um nome de arquivo único para a imagem .jpg, utilizando um GUID e o nome normalizado do artista.
    /// </summary>
    /// <param name="nomeNormalizado">Nome do arquivo normalizado</param>
    /// <returns></returns>
    private string GerarNomeArquivoJpg(string nomeNormalizado)
    {
        var guid = Guid.NewGuid();
        return $"{guid}_{nomeNormalizado}.jpg";
    }

}

