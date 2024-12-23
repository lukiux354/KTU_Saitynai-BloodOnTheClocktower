﻿using Saitynai.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace Saitynai.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }

        public Character Character { get; set; }

        [Required]
        public required string UserId { get; set; }
        public ForumUser User { get; set; }

        public CommentDto ToDto()
        {
            return new CommentDto(Id, Content, CreatedAt);
        }
    }
}
