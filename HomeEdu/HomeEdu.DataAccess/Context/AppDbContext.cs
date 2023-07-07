using HomeEdu.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace HomeEdu.DataAccess.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Blog> Blogs { get; set; } = null!;
        public DbSet<testimonial> testimonials { get; set; } = null!;
        public DbSet<Slider> Sliders { get; set; } = null!;
        public DbSet<NoticeBoard> noticeBoards { get; set; } = null!;
        public DbSet<BlogCatagory> BlogCatagories { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<EventDetail> EventDetails { get; set; } = null!;
        public DbSet<Speaker> Speakers { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<CourseDetail> CourseDetails { get; set; } = null!;
        public DbSet<CourseCatagory> CourseCatagories { get; set; } = null!;
        public DbSet<Language> Languages { get; set; } = null!;
        public DbSet<SkillLevel> SkillLevels { get; set; } = null!;
        public DbSet<Assesments> Assesments { get; set; } = null!;
    }
}
