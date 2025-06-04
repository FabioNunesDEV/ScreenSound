namespace ScreenSound.WEB.bootstrap.Responses;

public record ArtistaBase64Response(int Id, string Nome, string Bio, string? FotoPerfil, string? Base64)
{
    public override string ToString()
    {
        return $"{this.Nome}";
    }
}

