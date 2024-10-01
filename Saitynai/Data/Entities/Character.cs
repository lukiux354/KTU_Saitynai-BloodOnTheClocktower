namespace Saitynai.Data.Entities
{
    public class Character
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Body { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }

        public Script Script { get; set; }

        public CharacterDto ToDto()
        {
            return new CharacterDto(Id, Title, Body, CreatedAt);
        }
    }
}
