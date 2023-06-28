using HomeEdu.Core.Entities;
using Microsoft.EntityFrameworkCore;
namespace HomeEdu.DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Blog> Blogs { get; set; } = null!;
        public DbSet<Slider> Sliders { get; set; } = null!;
        public DbSet<BlogCatagory> BlogCatagories { get; set; } = null!;
    }
}
