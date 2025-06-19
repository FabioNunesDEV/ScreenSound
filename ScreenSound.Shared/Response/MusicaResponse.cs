namespace ScreenSound.Shared.Response;

public record MusicaResponse(
    int Id,
    string Nome,
    int ArtistaId,
    string NomeArtista,
    List<string> Generos
    );