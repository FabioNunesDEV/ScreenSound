namespace ScreenSound.API.EndPoints;
public static class ArtistasExtensions
{
    public static void AddEndPointsArtistas(this WebApplication app)
    {
        app.MapGet("/artistas", ([FromServices] DAL<Artista> dal) =>
        {
            var artistas = dal.Listar(a => a.Nome);

            return Results.Ok(artistas);
        });

        app.MapGet("/artistas/id/{id:int}",
            ([FromServices] DAL<Artista> dal,
            [FromServices] StorageService storageService,
            int id) =>
        {
            var artista = dal.RecuperarPor(a => a.Id == id);
            if (artista is null)
            {
                return Results.NotFound();
            }

            ArtistaResponse resposta =  new ArtistaResponse(
                artista.Id,
                artista.Nome,
                artista.Bio,
                artista.FotoPerfil
            );

            return Results.Ok(resposta);
        });

        app.MapGet("/artistas/nome/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
        {
            var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

            if (artista is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(artista);
        });

        app.MapPost("/artistas", (
            [FromServices] IHostEnvironment env,
            [FromServices] DAL<Artista> dal,
            [FromServices] StorageService storageService,
            [FromBody] ArtistaRequest artistaRequest) =>
        {
            var nome = artistaRequest.Nome.Trim();

            var artista = new Artista(artistaRequest.Nome, artistaRequest.Bio)
            { 
                FotoPerfil = artistaRequest.FotoPerfil ?? "card_padrao.jpg"
            };

            dal.Adicionar(artista);

            return Results.Ok();
        });

        app.MapDelete("/artistas/{id}", ([FromServices] DAL<Artista> dal, int id) =>
        {
            var artista = dal.RecuperarPor(a => a.Id == id);
            if (artista is null)
            {
                return Results.NotFound();
            }

            dal.Deletar(artista);
            return Results.NoContent();
        });

        app.MapPut("/artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequestEdit artistaRequestEdit) =>
        {
            var artistaAAtualizar = dal.RecuperarPor(a => a.Id == artistaRequestEdit.Id);

            if (artistaAAtualizar is null)
            {
                return Results.NotFound();
            }
            artistaAAtualizar.Nome = artistaRequestEdit.Nome;
            artistaAAtualizar.Bio = artistaRequestEdit.Bio;
            artistaAAtualizar.FotoPerfil = artistaRequestEdit.FotoPerfil;

            dal.Atualizar(artistaAAtualizar);
            return Results.Ok();
        });
    }

}
