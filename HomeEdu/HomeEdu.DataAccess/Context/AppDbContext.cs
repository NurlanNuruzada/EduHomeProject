﻿using HomeEdu.Core.Entities;
using Microsoft.EntityFrameworkCore;
namespace HomeEdu.DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Blog> Blogs { get; set; } = null!;
    }
}
