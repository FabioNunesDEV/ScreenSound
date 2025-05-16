using System.ComponentModel.DataAnnotations;

namespace ScreenSound.WEB.bootstrap.Requests;
public record ArtistaRequest([Required] string nome, [Required] string bio, string? fotoPerfil);
