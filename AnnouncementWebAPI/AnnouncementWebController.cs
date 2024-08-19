using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AnnouncementWebAPI.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace AnnouncementWebAPI
{
    public class AnnouncementWebController : Controller
    {

        private readonly HttpClient _httpClient;

        public AnnouncementWebController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var response = await  _httpClient.GetAsync("https://localhost:44353/api/announcements");
            response.EnsureSuccessStatusCode();

            var jsonResponse =  response.Content.ReadAsStringAsync();
            var announcements = JsonConvert.DeserializeObject<List<Announcement>>(jsonResponse.ToString());

            return View(announcements);
        }

        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:44353/api/announcements", announcement);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            return View(announcement);
        }

        /*
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44353/api/announcements/{id}");
            response.EnsureSuccessStatusCode();

            var announcement = await response.Content.ReadAsStringAsync<Announcement>();
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }
        */

        /*
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44353/api/ads/{id}");
            response.EnsureSuccessStatusCode();

            var ad = await response.Content.ReadAsAsync<>();
            if (ad == null)
            {
                return NotFound();
            }

            return View(ad);
        }
        */

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Announcement announcement)
        {
            if (id != announcement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:44353/api/announcements/{id}", announcement);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            return View(announcement);
        }

        /*
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44353/api/announcements/{id}");
            response.EnsureSuccessStatusCode();

            var ad = await response.Content.ReadAsAsync<Ad>();
            if (ad == null)
            {
                return NotFound();
            }

            return View(ad);
        }

        */

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:44353/api/announcements/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
    }
}

