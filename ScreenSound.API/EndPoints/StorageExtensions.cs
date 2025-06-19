namespace ScreenSound.API.EndPoints
{
    public static class StorageExtensions
    {
        public static void AddStorageEndpoints(this WebApplication app)
        {
            app.MapPost("/storage/upload", async (
                [FromServices] StorageService storageService,
                [FromBody] StorageUploadRequest request) =>
            {
                try
                {
                    await storageService.SalvarImagemBase64Async(
                        request.Base64Imagem,
                        request.NomeArquivo
                    );

                    return Results.Ok($"Arquivo {request.NomeArquivo} gravado com sucesso.");
                }
                catch (Exception ex)
                {
                    // Retorna uma resposta padrão de erro (HTTP 500)
                    return Results.Problem(
                        detail: ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }
            });

            app.MapGet("/storage/download/{nomeArquivo}", async (
                [FromServices] StorageService storageService,
                string nomeArquivo) =>
            {
                try
                {
                    var base64 = await storageService.LerBase64Async(nomeArquivo);
                    return Results.Ok(base64);
                }
                catch (FileNotFoundException ex)
                {
                    // Se o arquivo não for encontrado, retorne 404
                    return Results.NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    // Retorna uma resposta padrão de erro (HTTP 500)
                    return Results.Problem(
                        detail: ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }
            });
        }
    }
}
