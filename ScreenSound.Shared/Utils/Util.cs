using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace ScreenSound.Shared.Utils;

public static class Util
{
    /// <summary>
    /// Método para normalizar uma string, removendo acentos, cedilha e caracteres especiais.
    /// </summary>
    /// <param name="texto">String a ser normalizada</param>
    /// <returns>Retorna uma string normalizada</returns>
    public static string NormalizarString(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return string.Empty;

        // Remove espaços em branco no início e no fim
        texto = texto.Trim(); 

        // Converte para minúsculo
        texto = texto.ToLowerInvariant();

        // Remove acentos e cedilha
        texto = texto.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in texto)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                // Substitui cedilha manualmente
                if (c == 'ç') sb.Append('c');
                else sb.Append(c);
            }
        }
        texto = sb.ToString().Normalize(NormalizationForm.FormC);

        // Remove caracteres especiais (mantém apenas letras, números e espaço)
        texto = Regex.Replace(texto, @"[^a-z0-9\s]", "");

        // Substitui espaços por underscore
        texto = Regex.Replace(texto, @"\s+", "_");

        return texto;
    }

    /// <summary>
    /// Gera um nome de arquivo único utilizando um prefixo/sufixo e o nome desejado, mantendo a extensão.
    /// </summary>
    /// <param name="originalFileName">Nome original do arquivo, incluindo extensão (exemplo: "imagem.jpg").</param>
    /// <returns>Nome do arquivo contendo um prefixo único.</returns>
    public static string GerarNomeUnicoArquivo(string originalFileName)
    {
        var prefix = Guid.NewGuid().ToString("N");
        var extension = Path.GetExtension(originalFileName);
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);

        return $"{prefix}_{nameWithoutExtension}{extension}";
    }

}
