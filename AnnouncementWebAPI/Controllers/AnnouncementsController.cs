using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnnouncementWebAPI.DTO;
using AnnouncementWebAPI.Services;

namespace AnnouncementWebAPI.Controllers
{
    [Route("api/Announcements")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly AnnouncementService Service;

        public AnnouncementsController(AnnouncementService service)
        {
            Service = service;
        }

        [HttpGet("similar")]

        public async Task<ActionResult<IEnumerable<AnnouncementDTO>>> GetSimilarAnnouncements()
        {
            var announcements = await Service.GetSimilarAnnouncementsAsync();
            return Ok(announcements);
        }

        // GET: api/Announcements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnnouncementDTO>>> GetAnnouncements()
        {
            var announcements = await Service.GetAnnouncementsAsync();
            return Ok(announcements);
        }

        // GET: api/Announcements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnnouncementDTO>> GetAnnouncement(int id)
        {
            var announcement = await Service.GetAnnouncementAsync(id);

            if (announcement == null)
            {
                return NotFound();
            }

            return Ok(announcement);
        }

        // PUT: api/Announcements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnnouncement(int id, [FromBody]AnnouncementDTO announcementDTO)
        {
            if (id != announcementDTO.Id)
            {
                return BadRequest();
            }

            await Service.UpdateAnnouncementAsync(id, announcementDTO);

            try
            {
                await Service.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Announcements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AnnouncementDTO>> PostAnnouncement([FromBody] AnnouncementDTO announcementDTO)
        {
            await Service.AddAnnouncementAsync(announcementDTO);

            return CreatedAtAction(nameof(GetAnnouncement), new { id = announcementDTO.Id }, announcementDTO);
        }

        // DELETE: api/Announcements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await Service.GetAnnouncementAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            await Service.DeleteAnnouncementAsync(announcement.Id);
            await Service.SaveAsync();

            return NoContent();
        }

        private bool AnnouncementExists(int id)
        {
            return Service.Exists(id);
        }
    }
}
