using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AnnouncementWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Diagnostics;

namespace AnnouncementWebAPI.Repositories
{
   
    public class AnnouncementRepository
    {
        public AnnouncementContext _context;

        public AnnouncementRepository(AnnouncementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncementsAsync()
        {
            return await _context.Announcements.ToListAsync();
        }

        public async Task<Announcement> GetAnnouncementAsync(int id)
        {
            return await _context.Announcements.FindAsync(id);

        }

        public async Task AddAnnouncementAsync(Announcement announcement)
        {
            _context.Announcements.Add(announcement);
            SaveAsync();

        }

        public async Task UpdateAnnouncementAsync(Announcement announcement)
        {
            _context.Entry(announcement).State = EntityState.Modified;
           
        }

        public async Task DeleteAnnouncementAsync(int id)
        {
            Announcement announcement = await _context.Announcements.FindAsync(id);

            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
               
            }

        }

        public bool Exists(int id)
        {
            return _context.Announcements.Any(e => e.Id == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
