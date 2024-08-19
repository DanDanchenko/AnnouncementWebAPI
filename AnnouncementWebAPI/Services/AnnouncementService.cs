using System.Collections.Generic;
using System.Threading.Tasks;
using AnnouncementWebAPI.DTO;
using AnnouncementWebAPI.Models;
using AnnouncementWebAPI.Repositories;

namespace AnnouncementWebAPI.Services
{
    public class AnnouncementService
    {
        public AnnouncementRepository repository;

        public AnnouncementService(AnnouncementRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<AnnouncementDTO>> GetAnnouncementsAsync()
        {
            var announcements = await repository.GetAnnouncementsAsync();
            var announcementDTOs = new List<AnnouncementDTO>();
            foreach (var announcement in announcements)
            {
                announcementDTOs.Add(new AnnouncementDTO { Id = announcement.Id, Title = announcement.Title, Description = announcement.Description, DateAdded = announcement.DateAdded });
            }
            return announcementDTOs;
        }

        public async Task<AnnouncementDTO> GetAnnouncementAsync(int id)
        {
            var announcement = await repository.GetAnnouncementAsync(id);

            if (announcement != null)
            {
                return new AnnouncementDTO { Id = announcement.Id, Title = announcement.Title, Description = announcement.Description, DateAdded = announcement.DateAdded };
            }
            else return null;
        }

        public async Task AddAnnouncementAsync(AnnouncementDTO announcementDTO)
        {
            var announcement = new Announcement { Title = announcementDTO.Title, Description = announcementDTO.Description, DateAdded = System.DateTime.Now };
            await repository.AddAnnouncementAsync(announcement);

        }

        public async Task UpdateAnnouncementAsync(int id, AnnouncementDTO announcementDTO)
        {
            var announcement = await repository.GetAnnouncementAsync(id);
            if (announcement != null)
            {
                announcement.Title = announcementDTO.Title;
                announcement.Description = announcementDTO.Description;
                announcement.DateAdded = System.DateTime.Now;
                await repository.UpdateAnnouncementAsync(announcement); 
            }
        }

        public async Task DeleteAnnouncementAsync(int id)
        {
            var announcement = await repository.GetAnnouncementAsync(id);

            if (announcement != null)
            {
                await repository.DeleteAnnouncementAsync(announcement.Id);
            }
        }

        public bool Exists(int id)
        {
           return repository.Exists(id);
        }

        public async Task SaveAsync()
            {

            await repository.SaveAsync(); 

         }

        public HashSet<string> GetWords(string AnnouncementText)
        {
            string[] Words = AnnouncementText.Split(new[] {' ', ',', '.', '!', '?', ';', ':'}, StringSplitOptions.RemoveEmptyEntries).Select(word => word.ToLower()).ToArray();
            HashSet<string> words = new HashSet<string>(Words);
            return words;
        }

        public int GetValueOfSimilarity(Announcement ann1, Announcement ann2)
        {
            HashSet<string> word1 = GetWords(ann1.Title + " " + ann1.Description);
            HashSet<string> word2 = GetWords(ann2.Title + " " + ann2.Description);

            return word1.Intersect(word2).Count();
        }

        public async Task<IEnumerable<AnnouncementDTO>> GetSimilarAnnouncementsAsync()
        {
            var announcements = await repository.GetAnnouncementsAsync();

            var Sorted = announcements.Select(announcement => new { Ann = announcement, SimilarityValue = announcements.Where(otherAnn => otherAnn.Id != announcement.Id).Sum(otherAnn => GetValueOfSimilarity(announcement, otherAnn)) })
                .OrderByDescending(announcementWithValue => announcementWithValue.SimilarityValue).Select(announcementWithValue => new AnnouncementDTO { Id = announcementWithValue.Ann.Id, Title = announcementWithValue.Ann.Title, Description = announcementWithValue.Ann.Description, DateAdded = announcementWithValue.Ann.DateAdded }).ToList();
            return Sorted;
        }
    }
}
