namespace ScreenSound.API.Models
{
    public class Musica
    {
        public Musica() { }

        public Musica(string nome)
        {
            Nome = nome;
        }

        public string Nome { get; set; }
        public int Id { get; set; }
        public int? AnoLancamento { get; set; }
        public int? ArtistaId { get; set; }

        [JsonIgnore]
        public virtual Artista? Artista { get; set; }
        public virtual ICollection<Genero> Generos { get; set; } = new List<Genero>();

        public void ExibirFichaTecnica()
        {
            Console.WriteLine($"Ficha Técnica da música {Nome}");
        }

        public override string ToString()
        {
            return $@"Id: {Id}
            Nome: {Nome}
            Ano de Lançamento: {AnoLancamento}";
        }
    }
}
