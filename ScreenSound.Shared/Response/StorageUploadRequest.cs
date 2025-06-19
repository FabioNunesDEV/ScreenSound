namespace ScreenSound.Shared.Response;

/// <summary>
/// DTO para receber os dados da imagem e do artista que originará o arquivo.
/// </summary>
public record StorageUploadRequest(string Base64Imagem, string NomeArquivo);
