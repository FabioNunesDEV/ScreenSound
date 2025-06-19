using ScreenSound.Shared.Utils;

namespace ScreenSound.BLL.StorageBLL;
public class StorageBLL
{
    /// <summary>
    /// Método para salvar uma imagem em Base64 no local informado, gerando um nome de arquivo único.
    /// </summary>
    /// <param name="base64Imagem">
    /// String representando a imagem em Base64 (exemplo: "iVBORw0KGgoAAAANSUhEUgA...")
    /// </param>
    /// <param name="diretorio">
    /// Caminho físico da pasta onde o arquivo será armazenado.
    /// </param>
    /// <param name="nomeArquivo">
    /// Nome do arquivo desejado (incluindo extensão).
    /// </param>
    public async Task SaveImageBase64Async(
        string base64Imagem,
        string diretorio,
        string nomeArquivo)
    {
        // Garante que a pasta existe
        VerificarDiretorioExiste(diretorio);

        // Caminho completo até o novo arquivo
        var fullPath = Path.Combine(diretorio, nomeArquivo);

        // Converte Base64 em array de bytes
        var imageBytes = Convert.FromBase64String(base64Imagem);

        // Grava a imagem em disco
        await using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
    }

    /// <summary>
    /// Lê um arquivo específico do diretório informado e retorna seu conteúdo em Base64.
    /// </summary>
    /// <param name="diretorio">Caminho do diretório onde o arquivo está salvo.</param>
    /// <param name="nomeArquivo">Nome do arquivo (com extensão) a ser lido.</param>
    /// <returns>String em Base64 representando o conteúdo do arquivo.</returns>
    public async Task<string> LerBase64DeArquivoAsync(string diretorio, string nomeArquivo)
    {
        var fullPath = Path.Combine(diretorio, nomeArquivo);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("Arquivo não encontrado.", fullPath);
        }

        var bytes = await File.ReadAllBytesAsync(fullPath);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Verifica se o diretório existe; caso contrário, cria.
    /// </summary>
    /// <param name="diretorio">Caminho da pasta.</param>
    private void VerificarDiretorioExiste(string diretorio)
    {
        if (!Directory.Exists(diretorio))
        {
            Directory.CreateDirectory(diretorio);
        }
    }
}

