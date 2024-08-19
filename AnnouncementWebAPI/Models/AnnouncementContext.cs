using Microsoft.EntityFrameworkCore;

namespace AnnouncementWebAPI.Models
{
    public class AnnouncementContext : DbContext
    {
        public AnnouncementContext(DbContextOptions<AnnouncementContext> options)
        : base(options)
        {
        }

        public DbSet<Announcement> Announcements { get; set; }
    }
}
