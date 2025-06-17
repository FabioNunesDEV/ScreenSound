namespace ScreenSound.WEB.bootstrap.Requests;
public record ArtistaRequestEdit(int Id, string nome, string bio, string? fotoPerfil, string base64)
    : ArtistaRequest(nome, bio, fotoPerfil, base64);