using ScreenSound.API.Services;

namespace ScreenSound.API.EndPoints;
public static class ArtistasExtensions
{
    public static void AddEndPointsArtistas(this WebApplication app)
    {
        app.MapGet("/artistas", async ([FromServices] DAL<Artista> dal, [FromServices] StorageService storageService) =>
        {
            var artistas = dal.Listar(a => a.Musicas);

            var resposta = new List<ArtistaBase64Response>();
            foreach (var artista in artistas)
            {
                var base64 = await storageService.ObterBase64Async(artista.FotoPerfil);
                resposta.Add(new ArtistaBase64Response(
                    artista.Id,
                    artista.Nome,
                    artista.Bio,
                    artista.FotoPerfil,
                    base64
                ));
            }

            return Results.Ok(resposta);
        });

        app.MapGet("/artistas/id/{id:int}",
            async ([FromServices] DAL<Artista> dal,
            [FromServices] StorageService storageService,
            int id) =>
        {
            var artista = dal.RecuperarPor(a => a.Id == id);
            if (artista is null)
            {
                return Results.NotFound();
            }

            var base64 = await storageService.ObterBase64Async(artista.FotoPerfil);

            ArtistaBase64Response resposta =  new ArtistaBase64Response(
                artista.Id,
                artista.Nome,
                artista.Bio,
                artista.FotoPerfil,
                base64
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

        app.MapPost("/artistas", async (
            [FromServices] IHostEnvironment env,
            [FromServices] DAL<Artista> dal,
            [FromServices] StorageService storageService,
            [FromBody] ArtistaRequest artistaRequest) =>
        {
            var nome = artistaRequest.nome.Trim();

            var nomeArquivo = await storageService.SalvarImagemBase64Async(artistaRequest.fotoPerfil!, artistaRequest.nome);

            var artista = new Artista(artistaRequest.nome, artistaRequest.bio)
            {
                FotoPerfil = nome
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
            artistaAAtualizar.Nome = artistaRequestEdit.nome;
            artistaAAtualizar.Bio = artistaRequestEdit.bio;
            artistaAAtualizar.FotoPerfil = artistaRequestEdit.fotoPerfil;

            dal.Atualizar(artistaAAtualizar);
            return Results.Ok();
        });
    }

    private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
    {
        return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
    }

    private static ArtistaResponse EntityToResponse(Artista artista)
    {
        return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
    }
}
