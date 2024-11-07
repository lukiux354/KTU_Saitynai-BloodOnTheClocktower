using Saitynai.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace Saitynai.Data.Entities
{
    public class Character
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Body { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }

        public Script Script { get; set; }

        [Required]
        public required string UserId { get; set; }
        public ForumUser User { get; set; }

        public CharacterDto ToDto()
        {
            return new CharacterDto(Id, Title, Body, CreatedAt);
        }
    }
}
