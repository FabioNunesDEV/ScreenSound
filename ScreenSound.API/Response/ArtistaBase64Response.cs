namespace ScreenSound.API.Response;

public record ArtistaBase64Response(
    int Id,
    string Nome,
    string Bio,
    string? FotoPerfil,
    string? Base64
);

