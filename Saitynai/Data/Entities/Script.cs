using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using Saitynai.Auth.Model;

namespace Saitynai.Data.Entities
{
    public class Script
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }

        //only can be set/seen by admin
        public bool IsBlocked { get; set; }

        [Required]
        public required string UserId { get; set; }
        public ForumUser User { get; set; }


        public ScriptDto ToDto()
        {
            return new ScriptDto(Id, Title, Description, CreatedAt);
        }
    }
}
