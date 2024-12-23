﻿using Microsoft.EntityFrameworkCore;
using Saitynai.Data.Entities;
using Microsoft.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Saitynai.Auth.Model;

namespace Saitynai.Data
{
    public class ForumDbContext : IdentityDbContext<ForumUser>
    {
        private readonly IConfiguration _configuration;
        public DbSet<Script> Scripts { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public ForumDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString(("PostgreSQL")));
        }
    }
}
